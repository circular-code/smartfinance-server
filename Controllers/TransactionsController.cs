using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using System.Text.Json;

// TODO import (create multiple) Transactions
// TODO delete multiple ransactions

namespace Smartfinance_server.Controllers

{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public TransactionsController(QueryEngine qe)
        {
            _qe = qe;
        }

        //GET api/transactions
        //get all transactions
        //TODO: limit with skip & take, filter etc. like devextreme params
        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetAllTransactions() {
            return Ok(_qe.GetAllTransactions());
        }

        //GET api/transactions/id
        //get a specific transaction
        [HttpGet("{id}")]
        public ActionResult<Transaction> GetTransaction(uint id)
        {
            var transaction = _qe.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        //POST api/transactions
        //create a transaction in full
        [HttpPost]
        public ActionResult CreateTransaction(Transaction transaction)
        {
            var newTransaction = _qe.CreateTransaction(transaction);

            if (newTransaction == null)
                return NotFound();
                
            return NoContent();
        }

        // PUT api/transactions/id
        // update transaction data (excluding id)
        // we are missusing PUT to Update existing transaction instead of PATCH since PATCH requres additional dependencies (jsonpatch) and a different endpoint-style; see https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        [HttpPut("{id}")]
        public ActionResult UpdateTransaction(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            var transaction = _qe.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            _qe.UpdateTransaction(id, updates);
            return NoContent();
        }

        // DELETE api/transactions/id
        // delete a specific transaction
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