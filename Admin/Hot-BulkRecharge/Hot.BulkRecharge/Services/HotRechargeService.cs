using Hot.API.Client.Models;
using Hot.API.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OneOf;
using Hot.API.Client.Common;
using OneOf.Types;
using System.Threading;

namespace Hot.BulkRecharge
{
    public class HotRechargeService
    {
        private readonly string baseUrl = "https://ssl.hot.co.zw/api/v1/";
        private readonly HotAPIClient client;
        public string AccessCode { get; set; }
        public string AccessPassword { get; set; }

        public HotRechargeService(HotAPIClient client)
        {
            this.client = client;
        }

        public async Task<WalletBalanceResponse> CheckHotWalletBalanceAsync()
        {
            client.SetOptions(baseUrl, AccessCode, AccessPassword, true);
            try
            {

                var result = await client.GetWalletBalanceAsync();
                return result.Match(balance => balance,
                    error =>
                    {
                        return new WalletBalanceResponse
                        {
                            ReplyMsg = error.Message,
                            ReplyCode = (int)ReplyCode.FailedWebException
                        };
                    },
                    content =>
                    {
                        return new WalletBalanceResponse
                        {
                            ReplyMsg = content,
                            ReplyCode = (int)ReplyCode.FailedWebException
                        };
                    });
            }
            catch (Exception ex)
            {
                return new WalletBalanceResponse
                {
                    ReplyMsg = ex.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }
        public async Task<WalletBalanceResponse> CheckHotWalletBalanceUSDAsync()
        {
            client.SetOptions(baseUrl, AccessCode, AccessPassword, true);
            try
            {

                var result = await client.GetWalletBalanceUSDAsync();
                return result.Match(balance => balance,
                    error =>
                    {
                        return new WalletBalanceResponse
                        {
                            ReplyMsg = error.Message,
                            ReplyCode = (int)ReplyCode.FailedWebException
                        };
                    },
                    content =>
                    {
                        return new WalletBalanceResponse
                        {
                            ReplyMsg = content,
                            ReplyCode = (int)ReplyCode.FailedWebException
                        };
                    });
            }
            catch (Exception ex)
            {
                return new WalletBalanceResponse
                {
                    ReplyMsg = ex.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        public async Task<RechargeResponse> RechargeDataBundle(BulkRecharge item)
        {
            client.SetOptions(baseUrl, AccessCode, AccessPassword, true);
            try
            {
                if (item.IsUSD)
                {
                    var result = item.HasCustomSMS
                    ? await client.RechargeDataUSDAsync(item.Mobile, item.ProductCode, item.Reference, item.CustomSMS)
                    : await client.RechargeDataUSDAsync(item.Mobile, item.ProductCode, item.Reference);
                    return ReturnResult(result);

                }
                else
                {
                    var result = item.HasCustomSMS
                    ? await client.RechargeDataAsync(item.Mobile, item.ProductCode, item.Reference, item.CustomSMS)
                    : await client.RechargeDataAsync(item.Mobile, item.ProductCode, item.Reference);
                    return ReturnResult(result);
                }

            }
            catch (Exception ex)
            {
                return new RechargeResponse
                {
                    ReplyMsg = ex.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }

        }

        public async Task<RechargeResponse> RechargeAirtime(BulkRecharge item)
        {

            client.SetOptions(baseUrl, AccessCode, AccessPassword, true);
            try
            {
                if (item.IsUSD)
                {
                    if (item.Mobile.StartsWith("071"))
                    {
                        var result =
                            await client.RechargeEvdUSDAsync(38, item.Amount, 1, item.Mobile, item.Reference);
                        return ReturnResult(result);
                    }
                    else
                    {
                        var result = item.HasCustomSMS
                    ? await client.RechargeUSDAsync(item.Mobile, item.Amount, item.Reference, item.CustomSMS)
                    : await client.RechargeUSDAsync(item.Mobile, item.Amount, item.Reference);
                        return ReturnResult(result);
                    }
                }
                else
                {
                    var result = item.HasCustomSMS
                   ? await client.RechargeAsync(item.Mobile, item.Amount, item.Reference, item.CustomSMS)
                   : await client.RechargeAsync(item.Mobile, item.Amount, item.Reference);
                    return ReturnResult(result);
                }

            }
            catch (Exception ex)
            {
                return new RechargeResponse
                {
                    ReplyMsg = ex.Message,
                    ReplyCode = (int)ReplyCode.FailedWebException
                };
            }
        }

        private RechargeResponse ReturnResult(OneOf<RechargeResponse, APIException, string> result)
        {

            return result.Match(
                response => response,
                error =>
                {
                    return new RechargeResponse
                    {
                        ReplyMsg = error.Message,
                        ReplyCode = (int)ReplyCode.FailedWebException
                    };
                },
                context =>
                {
                    return new RechargeResponse
                    {
                        ReplyMsg = context,
                        ReplyCode = (int)ReplyCode.FailedWebException
                    };
                });
        }
        private RechargeResponse ReturnResult(OneOf<BulkEvdResponse, APIException, string> result)
        { 
            return result.Match(
                response =>
                {
                    return new RechargeResponse
                    {
                        ReplyMsg = response.ReplyMsg,
                        Amount = response.Amount,
                        AgentReference = response.AgentReference,
                        Discount = response.Discount,
                        WalletBalance = response.WalletBalance,
                        RechargeID = response.RechargeID,
                        ReplyCode = response.ReplyCode,
                        Data = string.Join(";", response.Pins)
                    };
                },
                error =>
                {
                    return new RechargeResponse
                    {
                        ReplyMsg = error.Message,
                        ReplyCode = (int)ReplyCode.FailedWebException
                    };
                },
                context =>
                {
                    return new RechargeResponse
                    {
                        ReplyMsg = context,
                        ReplyCode = (int)ReplyCode.FailedWebException
                    };
                });
        }

    }
}
