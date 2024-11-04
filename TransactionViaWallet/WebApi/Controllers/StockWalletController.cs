//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebAPI.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StockWalletController : ControllerBase
//    {
//        private readonly AuthenticationContext _context;

//        public StockWalletController(AuthenticationContext context)
//        {
//            _context = context;
//        }

//        // GET: api/StockWallet
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<StockWalletModel>>> GetStockWallets()
//        {
//            return await _context.StockWallets.ToListAsync();
//        }

//        // GET: api/StockWallet/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<StockWalletModel>> GetStockWallet(int id)
//        {
//            var stockWallet = await _context.StockWallets.FindAsync(id);

//            if (stockWallet == null)
//            {
//                return NotFound();
//            }

//            return stockWallet;
//        }

//        // PUT: api/StockWallet/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutStockWallet(int id, StockWalletModel stockWallet)
//        {
//            if (id != stockWallet.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(stockWallet).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!StockWalletExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/StockWallet
//        [HttpPost]
//        public async Task<ActionResult<StockWalletModel>> PostStockWallet(StockWalletModel stockWallet)
//        {
//            // Suppression de la valeur explicite de l'Id si elle est définie
//            stockWallet.Id = 0;

//            _context.StockWallets.Add(stockWallet);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetStockWallet", new { id = stockWallet.Id }, stockWallet);
//        }

//        // DELETE: api/StockWallet/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteStockWallet(int id)
//        {
//            var stockWallet = await _context.StockWallets.FindAsync(id);
//            if (stockWallet == null)
//            {
//                return NotFound();
//            }

//            _context.StockWallets.Remove(stockWallet);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool StockWalletExists(int id)
//        {
//            return _context.StockWallets.Any(e => e.Id == id);
//        }
//    }
//}
