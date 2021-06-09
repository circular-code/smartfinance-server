using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

// TODO import (create multiple) Transactions
// TODO delete multiple ransactions

namespace Smartfinance_server.Controllers

{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public TransactionController(QueryEngine qe)
        {
            _qe = qe;
        }

        //GET api/transaction
        //get all transactions
        //TODO: limit with skip & take, filter etc. like devextreme params
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetAllTransactions() {
            return Ok(_qe.GetAllTransactions());
        }

        //GET api/transaction/id
        //get a specific transaction
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Transaction> GetTransaction(uint id)
        {
            var transaction = _qe.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        //POST api/transaction
        //create a transaction in full
        [Authorize]
        [HttpPost]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            var newTransaction = _qe.CreateTransaction(transaction);

            if (newTransaction == null)
                return NotFound();
                
            return Ok(newTransaction);
        }

        // PUT api/transaction/id
        // update transaction data (excluding id)
        // we are missusing PUT to Update existing transaction instead of PATCH since PATCH requres additional dependencies (jsonpatch) and a different endpoint-style; see https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateTransaction(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            var transaction = _qe.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            _qe.UpdateTransaction(id, updates);
            return NoContent();
        }

        // DELETE api/transaction/id
        // delete a specific transaction
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteTransaction(uint id)
        {
            var transaction = _qe.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            _qe.DeleteTransaction(id);
            return NoContent();
        }
    }
}