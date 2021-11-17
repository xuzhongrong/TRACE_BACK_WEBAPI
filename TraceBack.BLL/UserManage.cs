using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TraceBack.Model;
using TraceBack.Model.UserManage;
using System.Linq;
using System.Data.SqlClient;

namespace TraceBack.BLL
{
    public class UserManage
    {
        public static string UserManageAPI(WebApiPara objPara)
        {
            WebApiResult apiResult = new WebApiResult();

            switch (objPara.ApiName)
            {
                case "Login":
                    {
                        Login(objPara, ref apiResult);
                    }
                    break;
                case "GetMenu":
                    {
                        GetMenu(objPara, ref apiResult);
                    }
                    break;
                case "GetUserList":
                    {
                        UserInfo query;
                        if (objPara.Data == null)
                        {
                            query = new UserInfo();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
                        }

                        List<UserInfo> users = Dal.UserManager.GetUserList(query);
                        apiResult.RecordCount = users.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = users;
                    }
                    break;
                case "GetEntList":
                    {
                        base_ent query;
                        if (objPara.Data == null)
                        {
                            query = new base_ent();
                        }
                        else
                        {
                            query = JsonConvert.DeserializeObject<base_ent>(objPara.Data.ToString());
                        }

                        List<base_ent> ents = Dal.UserManager.GetEntList(query);
                        apiResult.RecordCount = ents.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = ents;
                    }
                    break;
                case "SaveUserInfo":
                    {
                        UserInfo user;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            user = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.UserManager.SaveUserInfo(user, transaction);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex;
                            }
                            connection.Close();
                        }

                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "保存成功";
                        apiResult.Data = null;
                    }
                    break;
                case "SetUserStatus":
                    {
                        UserInfo user;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            user = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
                        }

                        Dal.UserManager.SetUserStatus(user);
                        apiResult.RecordCount = 0;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = null;
                    }
                    break;
                default:
                    throw new ArgumentException("未知的API接口");
            }

            return JsonConvert.SerializeObject(apiResult);
        }

        private static void Login(WebApiPara objPara, ref WebApiResult apiResult)
        {
            if (objPara.Data == null)
            {
                throw new ArgumentNullException("参数不能为空");
            }

            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
            apiResult.Data = userInfo;
            apiResult.RecordCount = 1;

            if (Dal.UserManager.Login(userInfo))
            {
                apiResult.Result = 1;
                apiResult.Message = "登录成功";
            }
            else
            {
                apiResult.Result = 0;
                apiResult.Message = "用户名或者密码错误，登录失败";
            }
        }

        private static void GetMenu(WebApiPara objPara, ref WebApiResult apiResult)
        {
            if (objPara.Data == null)
            {
                throw new ArgumentNullException("参数不能为空");
            }

            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
            if (userInfo.user_id == null)
            {
                throw new ArgumentNullException("用户ID不能为空");
            }

            List<Menu> menus = Dal.UserManager.GetMenu(userInfo.user_id.Value);
            apiResult.RecordCount = menus.Count;
            apiResult.Result = 1;
            apiResult.Message = "菜单查询成功";
            apiResult.Data = menus;
        }

        private static WebApiResult Login(WebApiPara objPara)
        {
            WebApiResult apiResult = new WebApiResult();

            if (objPara.Data == null)
            {
                throw new ArgumentNullException("参数不能为空");
            }

            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(objPara.Data.ToString());
            apiResult.Data = userInfo;
            apiResult.RecordCount = 1;

            if (Dal.UserManager.Login(userInfo))
            {
                apiResult.Result = 1;
                apiResult.Message = "登录成功";
            }
            else
            {
                apiResult.Result = 0;
                apiResult.Message = "用户名或者密码错误，登录失败";
            }

            return apiResult;
        }
    }
}
