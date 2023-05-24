using EPM.Mouser.Interview.Data;
using EPM.Mouser.Interview.Models;
using EPM.Mouser.Interview.Web.Pages;
using EPM.Mouser.Interview.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPM.Mouser.Interview.Web.Controllers
{
    [ApiController]
    [Route("api/warehouse")]
    public class WarehouseApi : ControllerBase
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseApi(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /*
         *  Action: GET
         *  Url: api/warehouse/id
         *  This action should return a single product for an Id
         */
        [HttpGet("{id}")]
        public async Task<JsonResult> GetProduct(long id)
        {
            // Could handle this a couple of ways.
            if(id < 0)
            {
                return new JsonResult(null);
            }

            var result = await _warehouseService.GetProduct(id);

            if(result is null)
            {
                return new JsonResult(null);
            }

            return new JsonResult(result);
        }

        /*
         *  Action: GET
         *  Url: api/warehouse
         *  This action should return a collection of products in stock
         *  In stock means In Stock Quantity is greater than zero and In Stock Quantity is greater than the Reserved Quantity
         */
        [HttpGet("")]
        public async Task<JsonResult> GetPublicInStockProducts()
        {
            var result = await _warehouseService.GetInStock();
            return new JsonResult(result);
        }


        /*
         *  Action: GET
         *  Url: api/warehouse/order
         *  This action should return a EPM.Mouser.Interview.Models.UpdateResponse
         *  This action should have handle an input parameter of EPM.Mouser.Interview.Models.UpdateQuantityRequest in JSON format in the body of the request
         *       {
         *           "id": 1,
         *           "quantity": 1
         *       }
         *
         *  This action should increase the Reserved Quantity for the product requested by the amount requested
         *
         *  This action should return failure (success = false) when:
         *     - ErrorReason.NotEnoughQuantity when: The quantity being requested would increase the Reserved Quantity to be greater than the In Stock Quantity.
         *     - ErrorReason.QuantityInvalid when: A negative number was requested
         *     - ErrorReason.InvalidRequest when: A product for the id does not exist
        */

        [HttpGet("order")]
        public async Task<JsonResult> OrderItem([FromBody] UpdateQuantityRequest request)
        {
            UpdateResponse response = new();

            if(request.Quantity < 0)
            {
                response.Success = false;
                response.ErrorReason = ErrorReason.QuantityInvalid;
                return new JsonResult(response);
            }
            
            var product = await _warehouseService.GetProduct(request.Id);
            
            if(product is null)
            {
                response.Success = false;
                response.ErrorReason = ErrorReason.InvalidRequest;
                return new JsonResult(response);
            }

            var itemOrdered = await _warehouseService.OrderItem(product, request.Quantity);

            if (!itemOrdered)
            {
                response.Success = false;
                response.ErrorReason = ErrorReason.QuantityInvalid;
                return new JsonResult(response);
            }

            response.Success = true;
            return new JsonResult(response);
        }

        /*
         *  Url: api/warehouse/ship
         *  This action should return a EPM.Mouser.Interview.Models.UpdateResponse
         *  This action should have handle an input parameter of EPM.Mouser.Interview.Models.UpdateQuantityRequest in JSON format in the body of the request
         *       {
         *           "id": 1,
         *           "quantity": 1
         *       }
         *
         *
         *  This action should:
         *     - decrease the Reserved Quantity for the product requested by the amount requested to a minimum of zero.
         *     - decrease the In Stock Quantity for the product requested by the amount requested
         *
         *  This action should return failure (success = false) when:
         *     - ErrorReason.NotEnoughQuantity when: The quantity being requested would cause the In Stock Quantity to go below zero.
         *     - ErrorReason.QuantityInvalid when: A negative number was requested
         *     - ErrorReason.InvalidRequest when: A product for the id does not exist
        */
        [HttpGet("ship")]
        public JsonResult ShipItem()
        {
            return new JsonResult(null);
        }

        /*
        *  Url: api/warehouse/restock
        *  This action should return a EPM.Mouser.Interview.Models.UpdateResponse
        *  This action should have handle an input parameter of EPM.Mouser.Interview.Models.UpdateQuantityRequest in JSON format in the body of the request
        *       {
        *           "id": 1,
        *           "quantity": 1
        *       }
        *
        *
        *  This action should:
        *     - increase the In Stock Quantity for the product requested by the amount requested
        *
        *  This action should return failure (success = false) when:
        *     - ErrorReason.QuantityInvalid when: A negative number was requested
        *     - ErrorReason.InvalidRequest when: A product for the id does not exist
        */
        [HttpGet("restock")]
        public JsonResult RestockItem()
        {
            return new JsonResult(null);
        }

        /*
        *  Url: api/warehouse/add
        *  This action should return a EPM.Mouser.Interview.Models.CreateResponse<EPM.Mouser.Interview.Models.Product>
        *  This action should have handle an input parameter of EPM.Mouser.Interview.Models.Product in JSON format in the body of the request
        *       {
        *           "id": 1,
        *           "inStockQuantity": 1,
        *           "reservedQuantity": 1,
        *           "name": "product name"
        *       }
        *
        *
        *  This action should:
        *     - create a new product with:
        *          - The requested name - But forced to be unique - see below
        *          - The requested In Stock Quantity
        *          - The Reserved Quantity should be zero
        *
        *       UNIQUE Name requirements
        *          - No two products can have the same name
        *          - Names should have no leading or trailing whitespace before checking for uniqueness
        *          - If a new name is not unique then append "(x)" to the name [like windows file system does, where x is the next avaiable number]
        *
        *
        *  This action should return failure (success = false) and an empty Model property when:
        *     - ErrorReason.QuantityInvalid when: A negative number was requested for the In Stock Quantity
        *     - ErrorReason.InvalidRequest when: A blank or empty name is requested
        */
        [HttpGet("add")]
        public JsonResult AddNewProduct()
        {
            return new JsonResult(null);
        }
    }
}
