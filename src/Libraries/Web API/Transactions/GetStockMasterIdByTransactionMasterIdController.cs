// ReSharper disable All
using MixERP.Net.Framework;
using MixERP.Net.Entities.Transactions;
using MixERP.Net.Schemas.Transactions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using PetaPoco;
using MixERP.Net.EntityParser;
namespace MixERP.Net.Api.Transactions
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetStockMasterIdByTransactionMasterId.
    /// </summary>
    [RoutePrefix("api/v1.5/transactions/procedures/get-stock-master-id-by-transaction-master-id")]
    public class GetStockMasterIdByTransactionMasterIdController : ApiController
    {
        /// <summary>
        /// Login id of application user accessing this API.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this API.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// Currently logged in office id of application user accessing this API.
        /// </summary>
        public int _OfficeId { get; set; }

        /// <summary>
        /// The name of the database where queries are being executed on.
        /// </summary>
        public string _Catalog { get; set; }

        /// <summary>
        ///     The GetStockMasterIdByTransactionMasterId repository.
        /// </summary>
        private readonly IGetStockMasterIdByTransactionMasterIdRepository repository;

        public class Annotation
        {
            public long StockMasterId { get; set; }
        }


        public GetStockMasterIdByTransactionMasterIdController()
        {
            this._LoginId = AppUsers.GetCurrent().View.LoginId.ToLong();
            this._UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            this._OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            this._Catalog = AppUsers.GetCurrentUserDB();

            this.repository = new GetStockMasterIdByTransactionMasterIdProcedure
            {
                _Catalog = this._Catalog,
                _LoginId = this._LoginId,
                _UserId = this._UserId
            };
        }

        public GetStockMasterIdByTransactionMasterIdController(IGetStockMasterIdByTransactionMasterIdRepository repository, string catalog, LoginView view)
        {
            this._LoginId = view.LoginId.ToLong();
            this._UserId = view.UserId.ToInt();
            this._OfficeId = view.OfficeId.ToInt();
            this._Catalog = catalog;

            this.repository = repository;
        }

        /// <summary>
        ///     Creates meta information of "get stock master id by transaction master id" annotation.
        /// </summary>
        /// <returns>Returns the "get stock master id by transaction master id" annotation meta information to perform CRUD operation.</returns>
        [AcceptVerbs("GET", "HEAD")]
        [Route("annotation")]
        [Route("~/api/transactions/procedures/get-stock-master-id-by-transaction-master-id/annotation")]
        public EntityView GetAnnotation()
        {
            if (this._LoginId == 0)
            {
                return new EntityView();
            }
            return new EntityView
            {
                Columns = new List<EntityColumn>()
                                {
                                        new EntityColumn { ColumnName = "_stock_master_id",  PropertyName = "StockMasterId",  DataType = "long",  DbDataType = "bigint",  IsNullable = false,  IsPrimaryKey = false,  IsSerial = false,  Value = "",  MaxLength = 0 }
                                }
            };
        }




        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/transactions/procedures/get-stock-master-id-by-transaction-master-id/execute")]
        public long Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.repository.StockMasterId = annotation.StockMasterId;


                return this.repository.Execute();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (MixERPException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}