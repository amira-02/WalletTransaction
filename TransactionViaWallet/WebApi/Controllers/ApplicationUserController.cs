using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private readonly AuthenticationContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings,
            AuthenticationContext context,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
     
        [Route("Register")]
        public async Task<IActionResult> Register(ApplicationUserModel model)
        {
            model.Role = "User";

            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.BankId == model.BankId && u.CIN == model.CIN && u.PhoneNumber == model.PhoneNumber.ToString());

            if (existingUser != null)
            {
                return BadRequest(new { message = "Un utilisateur avec ces informations existe déjà." });
            }

            var applicationUser = new ApplicationUser()
            {
                BankId = model.BankId,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber.ToString(),
                CIN = model.CIN,
                BankCode = model.BankCode,
                Status = model.Status,
                InscriptionDate = model.InscriptionDate,
                Role = model.Role,
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, model.Role);

                    var stockWallet = await _context.StockWallets.FirstOrDefaultAsync(sw => sw.BankCode == model.BankCode && sw.Status == "libre");

                    if (stockWallet == null)
                    {
                        return BadRequest(new { message = "Tous les StockWallets pour ce BankCode sont réservés. Impossible de créer un nouvel utilisateur." });
                    }

                    var walletNumber = "1" + model.BankCode + stockWallet.Stock.ToString("D3");

                    var wallet = new WalletModel
                    {
                        NumWallet = walletNumber,
                        Validity = "Valide ",
                        WalletStatus = "Active",
                        Solde = 0,
                        CIN = applicationUser.CIN
                    };

                    _context.Wallets.Add(wallet);

                    stockWallet.Stock++;
                    if (stockWallet.Stock == 999)
                    {
                        stockWallet.Status = "reserved";
                    }
                    _context.StockWallets.Update(stockWallet);

                    await _context.SaveChangesAsync();

                    var token = await _jwtTokenService.GenerateJwtTokenAsync(applicationUser);
                    return Ok(new { token });
                }
                else
                {
                    var errors = result.Errors.Select(e => new { code = e.Code, description = e.Description });
                    if (errors.Any(e => e.code == "DuplicateUserName"))
                    {
                        return BadRequest(new { message = "Ce nom d'utilisateur est déjà pris. Veuillez en choisir un autre." });
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await _jwtTokenService.GenerateJwtTokenAsync(user);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Nom d'utilisateur ou mot de passe incorrect." });
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(string id, ApplicationUserModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé." });
                }

                if (model.BankCode.ToString().Length != 3)
                {
                    return BadRequest(new { message = "BankCode doit être un code à 3 chiffres." });
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber.ToString();
                user.CIN = model.CIN;
                user.BankCode = model.BankCode;
                user.Status = model.Status;
                user.InscriptionDate = model.InscriptionDate;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Utilisateur mis à jour avec succès." });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
