using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TraceBack.Model;

namespace TraceBack.Dal
{
    public class SysManage
    {
        public static List<Menu> GetMenuList(Menu query, SqlTransaction transaction = null)
        {
            string strSql = @"SELECT m1.menu_id
                                    ,m1.menu_name_cn
                                    ,m1.menu_name_en
                                    ,m1.menu_parent
                                    ,m2.menu_name_cn as parent_name
                                    ,m1.menu_url
                                    ,m1.menu_icon
                                    ,m1.menu_level
                                    ,m1.display_order
                                    ,m1.menu_title
                                    ,m1.menu_remark
                                    ,m1.row_state
                                    ,m1.create_time
                                    ,m1.create_user
                                    ,m1.upd_time
                                    ,m1.upd_user
                                FROM base_menu m1
                                left join base_menu m2 on m2.menu_id = m1.menu_parent ";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.menu_name_cn))
                {
                    lstCondition.Add(" menu_name_cn like @user_name ");
                    lstPara.Add(new SqlParameter("@menu_name_cn", SqlDbType.VarChar) { Value = "%" + query.menu_name_cn + "%" });
                }

                if (query.menu_parent != null)
                {
                    lstCondition.Add(" menu_parent = @menu_parent ");
                    lstPara.Add(new SqlParameter("@menu_parent", SqlDbType.UniqueIdentifier) { Value = query.menu_parent });
                }

                if (query.menu_level != null)
                {
                    lstCondition.Add(" menu_level = @menu_level ");
                    lstPara.Add(new SqlParameter("@menu_level", SqlDbType.Int) { Value = query.menu_level });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }
                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            // 返回查询结果
            return DbUtility.SqlHelper.ExecuteList<Menu>(strSql, CommandType.Text, null, lstPara.ToArray());
        }
        public static int SaveMenu(Menu menu, SqlTransaction transaction = null)
        {
            if (menu == null
                || string.IsNullOrEmpty(menu.menu_name_cn)
                || string.IsNullOrEmpty(menu.menu_url)
            )
            {
                throw new ArgumentException("以下参数必需：课程名、地址");
            }
            List<SqlParameter> lstPara = new List<SqlParameter>();
            if (menu.menu_id == null || menu.menu_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@menu_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@menu_id", SqlDbType.UniqueIdentifier) { Value = menu.menu_id });
            }
            lstPara.Add(new SqlParameter("@menu_name_cn", SqlDbType.VarChar) { Value = menu.menu_name_cn });
            lstPara.Add(new SqlParameter("@menu_name_en", SqlDbType.VarChar) { Value = menu.menu_name_en });
            lstPara.Add(new SqlParameter("@menu_url", SqlDbType.VarChar) { Value = menu.menu_url });
            if (menu.menu_parent == null || menu.menu_parent == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@menu_parent", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@menu_parent", SqlDbType.UniqueIdentifier) { Value = menu.menu_parent });
            }
            lstPara.Add(new SqlParameter("@menu_remark", SqlDbType.VarChar) { Value = menu.menu_remark });
            lstPara.Add(new SqlParameter("@menu_title", SqlDbType.VarChar) { Value = menu.menu_title });
            lstPara.Add(new SqlParameter("@menu_url", SqlDbType.VarChar) { Value = menu.menu_url });
            lstPara.Add(new SqlParameter("@menu_level", SqlDbType.Int) { Value = menu.menu_level });
            lstPara.Add(new SqlParameter("@display_order", SqlDbType.Int) { Value = menu.display_order });
            lstPara.Add(new SqlParameter("@menu_icon", SqlDbType.VarChar) { Value = menu.menu_icon });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveMenu", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }
        public static List<base_role> GetRoleList(base_role role, SqlTransaction transaction = null)
        {
            string strSql = @"SELECT role_id
                                  ,role_name
                                  ,role_remark
                                  ,row_state
                                  ,create_time
                                  ,create_user
                                  ,upd_time
                                  ,upd_user
                              FROM base_role";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (role != null)
            {
                if (role.role_id != null)
                {
                    lstCondition.Add(" role_id = @role_id ");
                    lstPara.Add(new SqlParameter("@role_id", SqlDbType.UniqueIdentifier) { Value = role.role_id });
                }

                if (!string.IsNullOrEmpty(role.role_name))
                {
                    lstCondition.Add(" role_name like @role_name ");
                    lstPara.Add(new SqlParameter("@role_name", SqlDbType.VarChar) { Value = "%" + role.role_name + "%" });
                }
            }

            // 为Sql文添加条件
            if (lstCondition.Count > 0)
            {
                strSql += " where ";
                foreach (string strCondition in lstCondition)
                {
                    strSql += strCondition + " and ";
                }

                strSql = strSql.Substring(0, strSql.Length - 4);
            }

            // 返回查询结果
            return DbUtility.SqlHelper.ExecuteList<base_role>(strSql, CommandType.Text, null, lstPara.ToArray());
        }
    }
}
