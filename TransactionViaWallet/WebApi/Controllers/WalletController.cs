using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Models;
using WebAPI.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WalletController(AuthenticationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/Wallet/CreateWallet
        [HttpPost("CreateWallet")]
        public async Task<ActionResult<WalletModel>> PostWallet(WalletModel wallet)
        {
            wallet.NumWallet = await GenerateUniqueWalletNumber(wallet.CIN);

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserWallets), new { id = wallet.NumWallet }, wallet);
        }

        private async Task<string> GenerateUniqueWalletNumber(int cin)
        {
            var user = await _context.Users.FindAsync(cin);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var applicationUser = user as ApplicationUser;
            if (applicationUser == null)
            {
                throw new Exception("User is not of type ApplicationUser");
            }

            string bankCode = applicationUser.BankCode.ToString().PadLeft(3, '0');
            string prefix = "1" + bankCode;

            string walletNumber;
            do
            {
                walletNumber = prefix + new Random().Next(0, 999999999).ToString("D9");
            }
            while (await _context.Wallets.AnyAsync(w => w.NumWallet == walletNumber));

            return walletNumber;
        }

        // GET: api/Wallet/GetUserWallets
        [HttpGet("GetUserWallets")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WalletModel>>> GetUserWallets()
        {
            try
            {
                // Récupérer l'ID de l'utilisateur connecté à partir du token JWT
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Utilisateur non autorisé." });
                }

                // Récupérer les informations de l'utilisateur connecté
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé." });
                }

                // Utiliser le CIN de l'utilisateur pour trouver tous les wallets correspondant
                var wallets = await _context.Wallets
                    .Where(w => w.CIN == user.CIN)
                    .ToListAsync();

                if (wallets == null || wallets.Count == 0)
                {
                    return NotFound(new { message = "Aucun wallet trouvé pour cet utilisateur." });
                }

                return Ok(wallets);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        // PUT: api/Wallet/UpdateWallet/{id}
        [HttpPut("UpdateWallet/{id}")]
        public async Task<IActionResult> PutWallet(string id, WalletModel wallet)
        {
            if (id != wallet.NumWallet)
            {
                return BadRequest();
            }

            _context.Entry(wallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Wallet/DeleteWallet/{id}
        [HttpDelete("DeleteWallet/{id}")]
        public async Task<IActionResult> DeleteWallet(string id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WalletExists(string id)
        {
            return _context.Wallets.Any(e => e.NumWallet == id);
        }

        // POST: api/StockWallet/CreateStockWallet
        [HttpPost("CreateStockWallet")]
        public async Task<ActionResult<StockWalletModel>> PostStockWallet(StockWalletModel stockWallet)
        {
            // Suppression de la valeur explicite de l'Id si elle est définie
            stockWallet.Id = 0;

            _context.StockWallets.Add(stockWallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStockWallet), new { id = stockWallet.Id }, stockWallet);
        }

        // DELETE: api/StockWallet/DeleteStockWallet/{id}
        [HttpDelete("DeleteStockWallet/{id}")]
        public async Task<IActionResult> DeleteStockWallet(int id)
        {
            var stockWallet = await _context.StockWallets.FindAsync(id);
            if (stockWallet == null)
            {
                return NotFound();
            }

            _context.StockWallets.Remove(stockWallet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/StockWallet/UpdateStockWallet/{id}
        [HttpPut("UpdateStockWallet/{id}")]
        public async Task<IActionResult> PutStockWallet(int id, StockWalletModel stockWallet)
        {
            if (id != stockWallet.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockWallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockWalletExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool StockWalletExists(int id)
        {
            return _context.StockWallets.Any(e => e.Id == id);
        }

        // GET: api/StockWallet/GetStockWallet/{id}
        [HttpGet("GetStockWallet/{id}")]
        public async Task<ActionResult<StockWalletModel>> GetStockWallet(int id)
        {
            var stockWallet = await _context.StockWallets.FindAsync(id);
            if (stockWallet == null)
            {
                return NotFound();
            }

            return stockWallet;
        }

        // GET: api/StockWallet/GetAllStockWallets
        [HttpGet("GetAllStockWallets")]
        public async Task<ActionResult<IEnumerable<StockWalletModel>>> GetStockWallets()
        {
            return await _context.StockWallets.ToListAsync();
        }




        // POST: api/Wallet/AlimentationWallet
        [HttpPost("AlimentationWallet")]
        [Authorize]
        public async Task<IActionResult> AlimentationWallet(decimal amount)
        {
            try
            {
                // Récupérer l'ID de l'utilisateur connecté
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Utilisateur non autorisé." });
                }

                // Récupérer les informations de l'utilisateur connecté
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé." });
                }

                // Trouver la wallet de l'utilisateur connecté en utilisant le CIN et le bankcode
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.CIN == user.CIN && w.NumWallet.StartsWith("1" + user.BankCode));
                if (wallet == null)
                {
                    return NotFound(new { message = "Wallet de l'utilisateur non trouvé." });
                }

                // Ajouter le montant à la solde actuelle
                wallet.Solde += amount;

                // Enregistrer les changements dans la base de données
                await _context.SaveChangesAsync();

                return Ok(new { message = "Fonds ajoutés avec succès.", solde = wallet.Solde });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }






        // POST: api/Wallet/TransferToBank
        [HttpPost("TransferToBank")]
        [Authorize]
        public async Task<IActionResult> TransferToBank(AlimentationWalletModel model)
        {
            try
            {
                // Récupérer l'ID de l'utilisateur connecté
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { message = "Utilisateur non autorisé." });
                }

                // Récupérer les informations de l'utilisateur connecté
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé." });
                }

                // Trouver la wallet de l'utilisateur connecté en utilisant le CIN et le bankcode
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.CIN == user.CIN && w.NumWallet.StartsWith("1" + user.BankCode));
                if (wallet == null)
                {
                    return NotFound(new { message = "Wallet de l'utilisateur non trouvé." });
                }

                // Vérifier si le solde est suffisant pour le transfert
                if (wallet.Solde < model.Amount)
                {
                    return BadRequest(new { message = "Solde insuffisant pour effectuer le transfert." });
                }

                // Soustraire le montant du solde actuel
                wallet.Solde -= model.Amount;

                // Ici, vous pouvez ajouter la logique pour transférer l'argent à la banque
                // Par exemple, créer une entrée dans une table Transactions ou appeler un service externe
                // var transaction = new TransactionModel
                // {
                //     NumWallet = model.NumWallet,
                //     Amount = model.Amount,
                //     TransactionType = "TransferToBank",
                //     Date = DateTime.UtcNow
                // };

                // _context.Transactions.Add(transaction);

                // Enregistrer les changements dans la base de données
                await _context.SaveChangesAsync();

                return Ok(new { message = "Transfert effectué avec succès.", solde = wallet.Solde });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // POST: api/Wallet/Transfer
        [HttpPost("Transfer")]
        [Authorize]
        public async Task<IActionResult> Transfer([FromBody] TransactionModel model)
        {
            // Obtenir l'identifiant de l'utilisateur connecté à partir du token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "Utilisateur non autorisé." });
            }

            // Récupérer les informations de l'utilisateur connecté
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Utilisateur non trouvé." });
            }

            // Trouver le portefeuille de l'utilisateur connecté
            var senderWallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.CIN == user.CIN && w.NumWallet.StartsWith("1" + user.BankCode));
            if (senderWallet == null)
            {
                return NotFound(new { message = "Portefeuille de l'utilisateur non trouvé." });
            }

            // Trouver le portefeuille du récepteur
            var receiverWallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.NumWallet == model.DestinationWalletNum);
            if (receiverWallet == null)
            {
                return NotFound(new { message = "Portefeuille du récepteur non trouvé." });
            }

            // Vérifier si le solde du portefeuille de l'expéditeur est suffisant
            if (senderWallet.Solde < model.Amount)
            {
                return BadRequest(new { message = "Solde insuffisant." });
            }

            // Effectuer la transaction
            senderWallet.Solde -= model.Amount;
            receiverWallet.Solde += model.Amount;

            // Sauvegarder les modifications dans la base de données
            _context.Wallets.Update(senderWallet);
            _context.Wallets.Update(receiverWallet);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Transaction réussie." });
        }
    }
}
