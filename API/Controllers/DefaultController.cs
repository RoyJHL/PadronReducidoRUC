using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models.Default.Search;
using RJHL.Globals;
using RJHL.Models;

namespace API.Controllers
{
    public class DefaultController : ApiController
    {

        [HttpPost]
        [Route("api/ruc")]
        public async Task<IHttpActionResult> Search([FromBody] SearchModel request)
        {
            ResponseViewModel.ResponseView oResponse = new ResponseViewModel.ResponseView();
            ResponseViewModel.Message oMessage = new ResponseViewModel.Message
            {
                Type = ResponseViewModel.Type.Warning
            };
            oResponse.Message = oMessage;

            try
            {
                HashSet<string> values = null;
                bool hasMultipleValues = true;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"_files\Data\padron_reducido_ruc.txt");

                if (!File.Exists(filePath))
                    oResponse.Message.Text = "El archivo del padrón de RUCs no existe.";
                else if (request == null || (request.Ruc == null && request.Rucs == null))
                    oResponse.Message.Text = "El parámetro con el valor o los valores de RUCs no existe.";
                else
                {
                    if (request.Rucs == null)
                        hasMultipleValues = false;

                    values = hasMultipleValues
                        ? new HashSet<string>(request.Rucs)
                        : new HashSet<string> { request.Ruc };

                    oResponse.Message.Type = ResponseViewModel.Type.Ok;
                }


                if (oResponse.Message.Type == ResponseViewModel.Type.Ok)
                {
                    TxtSearcherGlobal txtSearcherGlobal = new TxtSearcherGlobal();
                    var result = await txtSearcherGlobal.SearchValuesAsync(
                        filePath,
                        values);

                    if (result.Count > 0)
                    {
                        if (hasMultipleValues)
                        {
                            Dictionary<string, SearchResponseModel> dSearchResponse =
                                new Dictionary<string, SearchResponseModel>();

                            foreach (var item in values)
                            {
                                SearchResponseModel searchResponseModel = null;
                                foreach (var kvp in result)
                                {
                                    if (item == kvp.Key)
                                    {
                                        string[] arrValue = kvp.Value.Split('|');
                                        searchResponseModel = new SearchResponseModel
                                        {
                                            Ruc = arrValue[0],
                                            NombreORazonSocial = arrValue[1],
                                            Estado = arrValue[2],
                                            Condicion = arrValue[3],
                                            UbigeoSunat = (arrValue[4] == "-" ? "" : arrValue[4]),
                                            Direccion = ((arrValue[5] == "-" || arrValue[5] == "----" ? "" : arrValue[5]) //TIPO DE VÍA
                                                          + (arrValue[6] == "-" ? "" : " " + arrValue[6]) //NOMBRE DE VÍA
                                                          + (arrValue[9] == "-" ? "" : " NRO. " + arrValue[9]) //NÚMERO
                                                          + (arrValue[7] == "-" || arrValue[7] == "----" ? "" : " " + arrValue[7]) //CÓDIGO DE ZONA
                                                          + (arrValue[8] == "-" ? "" : " " + arrValue[8]) //TIPO DE ZONA
                                                          + (arrValue[12] == "-" ? "" : " DPTO. " + arrValue[12])).TrimStart() //DEPARTAMENTO
                                        };

                                        result.Remove(kvp.Key);
                                        break;
                                    }
                                }
                                dSearchResponse.Add(item, searchResponseModel);
                            }
                            oResponse.Data = dSearchResponse;
                        }
                        else
                        {
                            foreach (var kvp in result)
                            {
                                string[] arrValue = kvp.Value.Split('|');
                                SearchResponseModel searchResponseModel = new SearchResponseModel
                                {
                                    Ruc = arrValue[0],
                                    NombreORazonSocial = arrValue[1],
                                    Estado = arrValue[2],
                                    Condicion = (arrValue[3] == "-" ? "" : arrValue[3]),
                                    UbigeoSunat = (arrValue[4] == "-" ? "" : arrValue[4]),
                                    Direccion = ((arrValue[5] == "-" || arrValue[5] == "----" ? "" : arrValue[5]) //TIPO DE VÍA
                                                 + (arrValue[6] == "-" ? "" : " " + arrValue[6]) //NOMBRE DE VÍA
                                                 + (arrValue[9] == "-" ? "" : " NRO. " + arrValue[9]) //NÚMERO
                                                 + (arrValue[7] == "-" || arrValue[7] == "----" ? "" : " " + arrValue[7]) //CÓDIGO DE ZONA
                                                 + (arrValue[8] == "-" ? "" : " " + arrValue[8]) //TIPO DE ZONA
                                                 + (arrValue[12] == "-" ? "" : " DPTO. " + arrValue[12])).TrimStart() //DEPARTAMENTO
                                };

                                oResponse.Data = searchResponseModel;
                                break;
                            }
                        }

                        oResponse.Message.Text = "Se realizó la búsqueda exitosamente.";
                    }
                    else
                    {
                        oResponse.Message.Type = ResponseViewModel.Type.Warning;
                        oResponse.Message.Text = "No hay resultados para su búsqueda.";
                    }
                }

            }
            catch (Exception ex)
            {
                oResponse.Message.Type = ResponseViewModel.Type.Error;
                oResponse.Message.Code = MessageViewModel.Error.Exception;
                oResponse.Message.Text = ex.Message;
                oResponse.Message.Status = MessageViewModel.Status.Exception;
                oResponse.Message.StackTrace = ex.StackTrace;
            }

            oResponse.Success = ResponseViewModel.IsSuccess(oResponse.Message.Type);

            return Ok(oResponse);
        }





    }
}
