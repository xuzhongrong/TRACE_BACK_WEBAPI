using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceBack.Model;
using TraceBack.Model.UserManage;

namespace TraceBack.BLL
{
    public class SysManage
    {
        public static string SysManageAPI(WebApiPara objPara)
        {
            WebApiResult apiResult = new WebApiResult();

            switch (objPara.ApiName)
            {
                case "GetMenuList":
                    {
                        Menu menu = null;
                        if (objPara.Data == null)
                        {
                            menu = new Menu();
                        }
                        else
                        {
                            menu = JsonConvert.DeserializeObject<Menu>(objPara.Data.ToString());
                        }

                        if (menu.PageSize == null)
                        {
                            menu.PageSize = 10;
                        }

                        if (menu.PageNumber == null)
                        {
                            menu.PageNumber = 1;
                        }

                        List<Menu> menus = Dal.SysManage.GetMenuList(menu);
                        List<Menu> topMenu = menus.Where(m => m.menu_level == 1).ToList();

                        topMenu.Sort(delegate (Menu x, Menu y)
                        {
                            return x.display_order.CompareTo(y.display_order);
                        });
                        //topMenu.Sort((x, y) => x.display_order.CompareTo(y.display_order));

                        List<Menu> sortedMenu = new List<Menu>();
                        foreach (Menu item in topMenu)
                        {
                            sortedMenu.Add(item);
                            List<Menu> subMenu = menus.Where(m => m.menu_parent == item.menu_id).ToList();
                            if (subMenu.Count > 0)
                            {
                                subMenu.Sort((x, y) => x.display_order.CompareTo(y.display_order));
                                sortedMenu.AddRange(subMenu);
                            }
                        }

                        apiResult.RecordCount = sortedMenu.Count;

                        // 分页
                        //sortedMenu = sortedMenu.Skip(menu.PageSize.Value * (menu.PageNumber.Value - 1)).Take(menu.PageSize.Value).ToList();

                        apiResult.Result = 1;
                        apiResult.Message = "菜单查询成功";
                        apiResult.Data = sortedMenu;
                    }
                    break;
                case "SaveMenu":
                    {
                        Menu menu;
                        if (objPara.Data == null)
                        {
                            throw new ArgumentNullException("参数不允许为空");
                        }
                        else
                        {
                            menu = JsonConvert.DeserializeObject<Menu>(objPara.Data.ToString());
                        }

                        using (SqlConnection connection = new SqlConnection(Dal.DbUtility.SqlHelper.ConnectionString))
                        {
                            connection.Open();
                            SqlTransaction transaction = connection.BeginTransaction();
                            try
                            {
                                Dal.SysManage.SaveMenu(menu, transaction);
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
                case "GetRoleList":
                    {
                        base_role role = null;
                        if (objPara.Data == null)
                        {
                            role = new base_role();
                        }
                        else
                        {
                            role = JsonConvert.DeserializeObject<base_role>(objPara.Data.ToString());
                        }

                        if (role.PageSize == null)
                        {
                            role.PageSize = 10;
                        }

                        if (role.PageNumber == null)
                        {
                            role.PageNumber = 1;
                        }

                        List<base_role> roles = Dal.SysManage.GetRoleList(role);

                        apiResult.RecordCount = roles.Count;
                        apiResult.Result = 1;
                        apiResult.Message = "查询成功";
                        apiResult.Data = roles;
                    }
                    break;
                default:
                    throw new ArgumentException("未知的API接口");
            }

            return JsonConvert.SerializeObject(apiResult);
        }
    }
}
