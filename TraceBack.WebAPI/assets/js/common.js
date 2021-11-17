var web_api_header = "http://localhost:61482/";

function onShowFoget() {
    $("#forgot-box").show();
    $("#login-box").hide();
}

function onShowLogin() {
    $("#forgot-box").hide();
    $("#login-box").show();}

// 登录事件
function onLogin() {
    // 接口参数
    var para = {
        ApiName: 'Login',
        Data: {
            user_name: $("#userName").val(),
            user_pwd: $("#pwd").val()
        }
    };

    if (para.Data.user_name === "") {
        layer.msg("请输入用户名！");
        return false;
    }

    if (para.Data.user_pwd === "") {
        layer.msg("请输入密码！");
        return false;
    }

    // 通过Ajax调用登录接口，完成登录，跳转到系统主页
    // ajax 调用后台
    $.ajax({
        url: web_api_header + "UserManage/UserMange",
        type: "post", //请求方式
        data: JSON.stringify(para),//数据，json字符串
        contentType: "application/json;charset=UTF-8",//请求的媒体类型
        success: function (result) {
            var resultJson = JSON.parse(result);
            if (resultJson.Result === 1) {
                localStorage.setItem("User", JSON.stringify(resultJson.Data));
                window.location.href = "/page/index.html";
            }
        }
    });
}

function getMenu(id) {

    // 接口参数
    var para = {
        ApiName: 'GetMenu',
        Data: {
            user_id: id
        }
    };

    // 通过Ajax调用登录接口，完成登录，跳转到系统主页
    $.ajax({
        //请求方式
        type: "POST",
        //请求的媒体类型
        contentType: "application/json;charset=UTF-8",
        //数据，json字符串
        data: JSON.stringify(para),
        //请求地址
        url: web_api_header + "UserManage/UserManage",
        //请求成功
        success: function (result) {
            var jsonResult = JSON.parse(result);
            if (jsonResult.Result === 1) {
                var menu = jsonResult.Data;
                var nav_list = $("#nav_list");
                var top_menu = $.grep(menu, function (m) {
                    return m.menu_level === 1;
                }).sort(function (a, b) {
                    return (a.display_order - b.display_order);
                });

                // 实现方法一 使用jQuery对象
                $.each(top_menu, function (i, top_item) {
                    var top_li = $('<li></li>');
                    if (top_item.menu_url === window.location.pathname) {
                        top_li.addClass("active");
                    }
                    var sub_menu = $.grep(menu, function (n) {
                        return (n.menu_level === 2 && n.menu_parent === top_item.menu_id);
                    }).sort(function (a, b) {
                        return (a.display_order - b.display_order);
                    });

                    if (sub_menu.length === 0) {
                        top_li.append(
                            $('<a></a>').attr("href", top_item.menu_url).attr("title", top_item.menu_title).append(
                                $('<i></i>').addClass("menu-icon").addClass(top_item.menu_icon).attr("aria-hidden", true)
                            ).append($('<span></span>').addClass("menu-text").html(top_item.menu_name_cn))).append(
                                $('<b></b>').addClass("arrow"));
                    } else {
                        top_li.append($('<a></a>').attr('href', top_item.menu_url).addClass("dropdown-toggle").append(
                            $('<i></i>').addClass("menu-icon").addClass(top_item.menu_icon).attr("aria-hidden", true)
                        ).append($('<span></span>').addClass("menu-text").html(top_item.menu_name_cn)).append(
                            $('<b></b>').addClass("arrow fa fa-angle-down"))).append(
                                $('<b></b>').addClass("arrow"));
                        var li_ul = $('<ul class="submenu"></ul>');
                        $.each(sub_menu, function (j, sub_item) {
                            var sub_li = $('<li></li>');
                            if (sub_item.menu_url === window.location.pathname) {
                                sub_li.addClass("active");
                                top_li.addClass("active open");
                            }
                            sub_li.append(
                                $('<a></a>').attr("href", sub_item.menu_url).append(
                                    $('<i></i>').addClass("menu-icon").addClass(sub_item.menu_icon).attr("aria-hidden", true)
                                ).append(
                                    $('<span></span>').addClass("menu-text").html(sub_item.menu_name_cn)
                                )).append($('<b></b>').addClass("arrow"));
                            li_ul.append(sub_li);
                        });
                        top_li.append(li_ul);
                    }
                    nav_list.append(top_li);
                });

                //// 实现方法二：使用字符串拼接
                //var menu_content = "";
                //var found_curr_page = false;
                //var page_url = window.location.pathname;
                //for (var i in top_menu) {
                //    var sub_menu = $.grep(menu, function (n) {
                //        return (n.menu_level == 2 && n.menu_parent == top_menu[i].menu_id);
                //    }).sort(function (a, b) {
                //        return (a.display_order - b.display_order);
                //    });

                //    //if (!found_curr_page && top_menu[i].menu_url == page_url) {
                //    //    menu_content += '<li class="active" >';
                //    //    found_curr_page = true;
                //    //} else {
                //    //    menu_content += '<li class="" >';
                //    //}

                //    if (sub_menu.length == 0) {
                //        menu_content += '<a href="' + top_menu[i].menu_url + '" title="' + top_menu[i].menu_title + '" >';
                //        menu_content += '<i class="menu-icon ' + top_menu[i].menu_icon + '" aria-hidden="true" ></i>';
                //        menu_content += '<span class="menu-text">' + top_menu[i].menu_name_cn + '</span>';
                //        menu_content += '</a>';
                //        menu_content += '<b class="arrow"></b>';
                //    } else {
                //        menu_content += '<a href="' + top_menu[i].menu_url + '" class="dropdown-toggle">';
                //        menu_content += '<i class="menu-icon ' + top_menu[i].menu_icon + '" ></i>';
                //        menu_content += '<span class="menu-text">' + top_menu[i].menu_name_cn + '</span>';
                //        menu_content += '<b class="arrow fa fa-angle-down"></b>';
                //        menu_content += '</a>';
                //        menu_content += '<b class="arrow"></b>';
                //        menu_content += '<ul class="submenu">';
                //        for (var j in sub_menu) {
                //            //if (!found_curr_page && sub_menu[j].menu_url == page_url) {
                //            //    menu_content += '<li class="active" >';
                //            //    found_curr_page = true;
                //            //} else {
                //            //    menu_content += '<li class="" >';
                //            //}
                //            menu_content += '<a href="' + sub_menu[j].menu_url + '">';
                //            menu_content += '<i class="menu-icon ' + sub_menu[j].menu_icon + '"></i>'
                //            menu_content += sub_menu[j].menu_name_cn;
                //            menu_content += ' </a>';
                //            menu_content += '<b class="arrow"></b>';
                //            menu_content += '</li>';
                //        }
                //        menu_content += '</ul>';
                //    }
                //    menu_content += '</li>';
                //}
                //nav_list.append(menu_content); 

            } else {
                // 菜单获取失败
                layer.msg(jsonResult.Message);
            }
        },
        //请求失败，包含具体的错误信息
        error: function (e) {
            console.log(e.status);
            console.log(e.responseText);
        }
    });
}

function initPage() {
    $("#forgot-box").hide();
    $("#login-box").show();
    $("#model_login").hide();
    var user = JSON.parse(localStorage.getItem("User"));
    if (user === null) {
        // 用户重新登录
        layer.open({
            type: 3,
            title: '用户信息',
            shadeClose: false, //点击遮罩关闭
            content: $('#model_login'),
            end: function () {
                $('#model_login').hide();
            }
        });

        $("#model_login").show();
    } else {
        $("#span_user_name").html(user.user_name);
        getMenu(user.user_id);
    }
}