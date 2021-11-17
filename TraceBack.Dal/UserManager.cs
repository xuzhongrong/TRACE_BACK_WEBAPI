using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TraceBack.Model;
using TraceBack.Model.UserManage;

namespace TraceBack.Dal
{
    public class UserManager
    {
        public static bool Login(UserInfo userInfo, SqlTransaction transaction = null)
        {
            if (string.IsNullOrEmpty(userInfo.user_name) || string.IsNullOrEmpty(userInfo.user_pwd))
            {
                return false;
            }

            string strSql = @"select user_id from base_user_info
                               where user_name = @user_name
                                 and user_pwd = @user_pwd
                                 and row_state = 1";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = userInfo.user_name });
            lstPara.Add(new SqlParameter("@user_pwd", SqlDbType.VarChar) { Value = userInfo.user_pwd });

            object objResult = DbUtility.SqlHelper.ExecuteScalar(strSql, CommandType.Text, null, lstPara.ToArray());
            if (objResult != null && objResult != DBNull.Value)
            {
                userInfo.user_id = ((Guid)objResult);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<UserInfo> GetUserList(UserInfo query, SqlTransaction transaction = null)
        {
            // sql文
            string strSql = @"select * from (
                                select u.user_id, u.user_name, u.user_email, u.user_phone, u.user_address,
                                       u.row_state, u.user_ent,
                                e.ent_name as ent_name,
                                stuff((
			                            select ',' + cast( r.role_id as varchar(50))
			                            from base_role r left join base_user_role ur on ur.role_id = r.role_id
			                            where ur.user_id = u.user_id
			                            for xml path('')
                                ), 1, 1, '') as role_id,
                                stuff((
			                            select ',' + r.role_name
			                            from base_role r left join base_user_role ur on ur.role_id = r.role_id
			                            where ur.user_id = u.user_id
			                            for xml path('')
                                ), 1, 1, '') as role_name
                                from base_user_info u 
                                left join base_ent e on u.user_ent = e.ent_id
                            ) user_info ";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (query != null)
            {
                if (query.user_id != null)
                {
                    lstCondition.Add(" user_info.user_id = @user_id ");
                    lstPara.Add(new SqlParameter("@user_id", SqlDbType.UniqueIdentifier) { Value = query.user_id });
                }

                if (!string.IsNullOrEmpty(query.user_name))
                {
                    lstCondition.Add(" user_info.user_name  like @user_name ");
                    lstPara.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = "%" + query.user_name + "%" });
                }

                if (!string.IsNullOrEmpty(query.user_email))
                {
                    lstCondition.Add(" user_info.user_email  like @user_email ");
                    lstPara.Add(new SqlParameter("@user_email", SqlDbType.VarChar) { Value = "%" + query.user_email + "%" });
                }

                if (!string.IsNullOrEmpty(query.ent_name))
                {
                    lstCondition.Add(" user_info.ent_name  like @ent_name ");
                    lstPara.Add(new SqlParameter("@ent_name", SqlDbType.VarChar) { Value = "%" + query.ent_name + "%" });
                }

                if (!string.IsNullOrEmpty(query.user_address))
                {
                    lstCondition.Add(" user_info.user_address  like @user_address ");
                    lstPara.Add(new SqlParameter("@user_address", SqlDbType.VarChar) { Value = "%" + query.user_address + "%" });
                }

                if (query.row_state != null)
                {
                    lstCondition.Add(" user_info.row_state = @row_state ");
                    lstPara.Add(new SqlParameter("@row_state", SqlDbType.VarChar) { Value = query.row_state });
                }

                if (!string.IsNullOrEmpty(query.role_name))
                {
                    lstCondition.Add(" user_info.role_name  like @user_role ");
                    lstPara.Add(new SqlParameter("@user_role", SqlDbType.VarChar) { Value = "%" + query.role_name + "%" });
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

            strSql += " order by user_info.user_name ";

            // 返回查询结果
            return DbUtility.SqlHelper.ExecuteList<UserInfo>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }

        public static int SetUserStatus(UserInfo user, SqlTransaction transaction = null)
        {
            if (user == null
                || user.user_id == null || user.user_id == Guid.Empty
                || user.row_state == null
                || user.upd_user == null || user.upd_user == Guid.Empty
            )
            {
                throw new ArgumentException("以下参数必需：用户ID、状态、修改者");
            }

            string strSql = @"UPDATE base_user_info
                                 SET row_state = @row_state,
                                     upd_time  = getdate(),
                                     upd_user  = @upd_user
                               WHERE user_id = @user_id";
            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@user_id", SqlDbType.UniqueIdentifier) { Value = user.user_id });
            lstPara.Add(new SqlParameter("@row_state", SqlDbType.Int) { Value = user.row_state });
            lstPara.Add(new SqlParameter("@upd_user", SqlDbType.UniqueIdentifier) { Value = user.upd_user });

            return DbUtility.SqlHelper.ExecuteNonQuery(strSql, CommandType.Text, transaction, lstPara.ToArray());
        }

        public static int SaveUserInfo(UserInfo user, SqlTransaction transaction = null)
        {
            if (user == null
                || string.IsNullOrEmpty(user.user_name)
                || string.IsNullOrEmpty(user.user_email)
                || string.IsNullOrEmpty(user.user_phone)
                || string.IsNullOrEmpty(user.role_id)
            )
            {
                throw new ArgumentException("以下参数必需：用户名、邮箱、电话、角色");
            }

            List<SqlParameter> lstPara = new List<SqlParameter>();

            if (user.user_id == null || user.user_id == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@user_id", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@user_id", SqlDbType.UniqueIdentifier) { Value = user.user_id });
            }
            lstPara.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = user.user_name });
            lstPara.Add(new SqlParameter("@user_email", SqlDbType.VarChar) { Value = user.user_email });
            lstPara.Add(new SqlParameter("@user_phone", SqlDbType.VarChar) { Value = user.user_phone });
            if (user.user_ent == null || user.user_ent == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@user_ent", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@user_ent", SqlDbType.UniqueIdentifier) { Value = user.user_ent });
            }
            lstPara.Add(new SqlParameter("@user_address", SqlDbType.VarChar) { Value = user.user_address });
            lstPara.Add(new SqlParameter("@role_id", SqlDbType.VarChar) { Value = user.role_id });
            if (user.upd_user == null || user.upd_user == Guid.Empty)
            {
                lstPara.Add(new SqlParameter("@upd_user", SqlDbType.UniqueIdentifier) { Value = DBNull.Value });
            }
            else
            {
                lstPara.Add(new SqlParameter("@upd_user", SqlDbType.UniqueIdentifier) { Value = user.upd_user });
            }
            lstPara.Add(new SqlParameter("@return", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SaveUserInfo", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p=>p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }

        public static int SetUserRole(Guid user_id, string role_ids, SqlTransaction transaction = null)
        {
            if (user_id == Guid.Empty || string.IsNullOrEmpty(role_ids))
            {
                throw new ArgumentException("以下参数必需：用户ID、角色");
            }

            List<SqlParameter> lstPara = new List<SqlParameter>();
            lstPara.Add(new SqlParameter("@user_id", SqlDbType.UniqueIdentifier) { Value = user_id });
            lstPara.Add(new SqlParameter("@role_ids", SqlDbType.UniqueIdentifier) { Value = role_ids });
            lstPara.Add(new SqlParameter("@return", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });

            // 返回执行结果
            DbUtility.SqlHelper.ExecuteNonQuery("SetUserRole", CommandType.StoredProcedure, transaction, lstPara.ToArray());

            object objRtn = lstPara.Where(p => p.ParameterName == "@return").FirstOrDefault().Value;

            return (int)objRtn;
        }

        public static List<base_ent> GetEntList(base_ent query, SqlTransaction transaction = null)
        {
            // sql文
            string strSql = @"SELECT ent_id
                                  ,ent_type
                                  ,ent_name
                                  ,ent_code
                                  ,ent_regist_address
                                  ,ent_busyness_address
                                  ,ent_legal_person_id
                                  ,ent_legal_person_name
                                  ,row_state
                                  ,create_time
                                  ,create_user
                                  ,upd_time
                                  ,upd_user
                              FROM base_ent";

            List<SqlParameter> lstPara = new List<SqlParameter>();
            List<string> lstCondition = new List<string>();

            // 组合条件
            if (query != null)
            {
                if (query.ent_id != null)
                {
                    lstCondition.Add(" ent_id = @ent_id ");
                    lstPara.Add(new SqlParameter("@ent_id", SqlDbType.UniqueIdentifier) { Value = query.ent_id });
                }

                if (!string.IsNullOrEmpty(query.ent_name))
                {
                    lstCondition.Add(" ent_name like @ent_name ");
                    lstPara.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = "%" + query.ent_name + "%" });
                }

                if (!string.IsNullOrEmpty(query.ent_address))
                {
                    lstCondition.Add(" (ent_registered_address like @ent_registered_address or ent_business_address = @ent_business_address) ");
                    lstPara.Add(new SqlParameter("@ent_registered_address", SqlDbType.VarChar) { Value = "%" + query.ent_address + "%" });
                    lstPara.Add(new SqlParameter("@ent_business_address", SqlDbType.VarChar) { Value = "%" + query.ent_address + "%" });
                }

                if (!string.IsNullOrEmpty(query.ent_name))
                {
                    lstCondition.Add(" user_info.ent_name  like @ent_name ");
                    lstPara.Add(new SqlParameter("@ent_name", SqlDbType.VarChar) { Value = "%" + query.ent_name + "%" });
                }

                if (query.row_state != null)
                {
                    lstCondition.Add(" row_state = @row_state ");
                    lstPara.Add(new SqlParameter("@row_state", SqlDbType.VarChar) { Value = query.row_state });
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

            strSql += " order by ent_type, ent_name";

            // 返回查询结果
            return DbUtility.SqlHelper.ExecuteList<base_ent>(
                    strSql, CommandType.Text, null, lstPara.ToArray());
        }

        public static List<Menu> GetMenu(Guid user_id, SqlTransaction transaction = null)
        {
            string strSql = @"	select distinct
	                                   m.menu_id,
		                               m.menu_name_cn, 
		                               m.menu_name_en, 
                                       m.menu_parent,
		                               m.menu_title, 
		                               m.menu_url, 
		                               m.menu_icon,
	                                   m.menu_level, 
		                               m.display_order
	                            from base_menu m
	                            left join base_role_menu rm on rm.menu_id = m.menu_id
	                            left join base_user_role ur on ur.role_id = rm.role_id
	                            where ur.user_id = @user_id
	                            order by m.display_order asc ";
            SqlParameter paraUserID = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier)
            {
                Value = user_id
            };

            // 返回查询结果
            return DbUtility.SqlHelper.ExecuteList<Menu>(strSql, CommandType.Text, null, paraUserID);
        }
    }
}
