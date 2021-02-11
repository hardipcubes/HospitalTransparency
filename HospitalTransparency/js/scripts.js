(function ($) {
    "use strict";
    $(document).ready(function () {

        //var id = document.getElementsByClassName('CheckActive');
        //if (id != undefined && id.length > 0) {
        //    var newid = id;
        //    SetActiveMenu(newid[0].id);
        //}

        

        $("#select-all").on("click", function () {
            var all = $(this);
            $('input:checkbox').each(function () {
                $(this).prop("checked", all.prop("checked"));
            });
        });

        $(document).ajaxStart(function () {
            $("#divLoading").css("display", "block");
        });

        $(document).ajaxComplete(function () {
            $("#divLoading").css("display", "none");
        });

        // remover space
        $("input").on('blur', function () {
            $(this).val($(this).val().trim());
        })
        $("textarea").on('blur', function () {
            $(this).val($(this).val().trim());
        })
        //set default img if img not found
        $("img").error(function () {
            $(this).unbind("error").attr("src", "../Content/images/notavailable.png");
        });
        //add numeric class for number validation 
        $('.numeric').on('input', function (event) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });

        //Disable paste
        $(".numeric").bind("paste", function (e) {
            e.preventDefault();
        });

        /*==Left Navigation Accordion ==*/
        if ($.fn.dcAccordion) {
            $('#nav-accordion').dcAccordion({
                eventType: 'click',
                autoClose: true,
                saveState: true,
                disableLink: true,
                speed: 'slow',
                showCount: false,
                autoExpand: true,
                classExpand: 'dcjq-current-parent'
            });
        }

        /*==Slim Scroll ==*/
        if ($.fn.slimScroll) {
            $('.event-list').slimscroll({
                height: '305px',
                wheelStep: 20
            });
            $('.conversation-list').slimscroll({
                height: '360px',
                wheelStep: 35
            });
            $('.to-do-list').slimscroll({
                height: '300px',
                wheelStep: 35
            });
        }

        /*==Nice Scroll ==*/
        if ($.fn.niceScroll) {

            $(".leftside-navigation").niceScroll({
                cursorcolor: "#1FB5AD",
                cursorborder: "0px solid #fff",
                cursorborderradius: "0px",
                cursorwidth: "3px"
            });

            $(".leftside-navigation").getNiceScroll().resize();
            if ($('#sidebar').hasClass('hide-left-bar')) {
                $(".leftside-navigation").getNiceScroll().hide();
            }
            $(".leftside-navigation").getNiceScroll().show();

            $(".right-stat-bar").niceScroll({
                cursorcolor: "#1FB5AD",
                cursorborder: "0px solid #fff",
                cursorborderradius: "0px",
                cursorwidth: "3px"
            });

        }

        ///*==Easy Pie chart ==*/
        //if ($.fn.easyPieChart) {

        //    $('.notification-pie-chart').easyPieChart({
        //        onStep: function (from, to, percent) {
        //            $(this.el).find('.percent').text(Math.round(percent));
        //        },
        //        barColor: "#39b6ac",
        //        lineWidth: 3,
        //        size: 50,
        //        trackColor: "#efefef",
        //        scaleColor: "#cccccc"
        //    });

        //    $('.pc-epie-chart').easyPieChart({
        //        onStep: function (from, to, percent) {
        //            $(this.el).find('.percent').text(Math.round(percent));
        //        },
        //        barColor: "#5bc6f0",
        //        lineWidth: 3,
        //        size: 50,
        //        trackColor: "#32323a",
        //        scaleColor: "#cccccc"
        //    });
        //}

        ///*== SPARKLINE==*/
        //if ($.fn.sparkline) {
        //    $(".d-pending").sparkline([3, 1], {
        //        type: 'pie',
        //        width: '40',
        //        height: '40',
        //        sliceColors: ['#e1e1e1', '#8175c9']
        //    });

        //    var sparkLine = function () {
        //        $(".sparkline").each(function () {
        //            var $data = $(this).data();
        //            ($data.type == 'pie') && $data.sliceColors && ($data.sliceColors = eval($data.sliceColors));
        //            ($data.type == 'bar') && $data.stackedBarColor && ($data.stackedBarColor = eval($data.stackedBarColor));

        //            $data.valueSpots = {
        //                '0:': $data.spotColor
        //            };
        //            $(this).sparkline($data.data || "html", $data);

        //            if ($(this).data("compositeData")) {
        //                $spdata.composite = true;
        //                $spdata.minSpotColor = false;
        //                $spdata.maxSpotColor = false;
        //                $spdata.valueSpots = {
        //                    '0:': $spdata.spotColor
        //                };
        //                $(this).sparkline($(this).data("compositeData"), $spdata);
        //            };
        //        });
        //    };

        //    var sparkResize;
        //    $(window).resize(function (e) {
        //        clearTimeout(sparkResize);
        //        sparkResize = setTimeout(function () {
        //            sparkLine(true)
        //        }, 500);
        //    });
        //    sparkLine(false);
        //}

        //if ($.fn.plot) {
        //    var datatPie = [30, 50];
        //    // DONUT
        //    $.plot($(".target-sell"), datatPie, {
        //        series: {
        //            pie: {
        //                innerRadius: 0.6,
        //                show: true,
        //                label: {
        //                    show: false
        //                },
        //                stroke: {
        //                    width: .01,
        //                    color: '#fff'
        //                }
        //            }
        //        },

        //        legend: {
        //            show: true
        //        },
        //        grid: {
        //            hoverable: true,
        //            clickable: true
        //        },
        //        colors: ["#ff6d60", "#cbcdd9"]
        //    });
        //}

        /*==Collapsible==*/
        $('.widget-head').click(function (e) {
            var widgetElem = $(this).children('.widget-collapse').children('i');
            $(this)
                .next('.widget-container')
                .slideToggle('slow');
            if ($(widgetElem).hasClass('ico-minus')) {
                $(widgetElem).removeClass('ico-minus');
                $(widgetElem).addClass('ico-plus');
            } else {
                $(widgetElem).removeClass('ico-plus');
                $(widgetElem).addClass('ico-minus');
            }
            e.preventDefault();
        });

        /*==Sidebar Toggle==*/
        $(".leftside-navigation .sub-menu > a").click(function () {
            var o = ($(this).offset());
            var diff = 80 - o.top;
            if (diff > 0)
                $(".leftside-navigation").scrollTo("-=" + Math.abs(diff), 500);
            //else
                //$(".leftside-navigation").scrollTo("+=" + Math.abs(diff), 500);
        });

        $('.sidebar-toggle-box .fa-bars').click(function (e) {
            $(".leftside-navigation").niceScroll({
                cursorcolor: "#1FB5AD",
                cursorborder: "0px solid #fff",
                cursorborderradius: "0px",
                cursorwidth: "3px"
            });

            $('#sidebar').toggleClass('hide-left-bar');
            if ($('#sidebar').hasClass('hide-left-bar')) {
                $(".leftside-navigation").getNiceScroll().hide();
            }
            $(".leftside-navigation").getNiceScroll().show();
            $('#main-content').toggleClass('merge-left');
            e.stopPropagation();
            if ($('#container').hasClass('open-right-panel')) {
                $('#container').removeClass('open-right-panel')
            }
            if ($('.right-sidebar').hasClass('open-right-bar')) {
                $('.right-sidebar').removeClass('open-right-bar')
            }

            if ($('.header').hasClass('merge-header')) {
                $('.header').removeClass('merge-header')
            }
        });

        $('.toggle-right-box .fa-bars').click(function (e) {
            $('#container').toggleClass('open-right-panel');
            $('.right-sidebar').toggleClass('open-right-bar');
            $('.header').toggleClass('merge-header');
            e.stopPropagation();
        });

        $('.header,#main-content,#sidebar').click(function () {
            if ($('#container').hasClass('open-right-panel')) {
                $('#container').removeClass('open-right-panel')
            }
            if ($('.right-sidebar').hasClass('open-right-bar')) {
                $('.right-sidebar').removeClass('open-right-bar')
            }

            if ($('.header').hasClass('merge-header')) {
                $('.header').removeClass('merge-header')
            }
        });

        $('.panel .tools .fa').click(function () {
            var el = $(this).parents(".panel").children(".panel-body");
            if ($(this).hasClass("fa-chevron-down")) {
                $(this).removeClass("fa-chevron-down").addClass("fa-chevron-up");
                el.slideUp(200);
            } else {
                $(this).removeClass("fa-chevron-up").addClass("fa-chevron-down");
                el.slideDown(200);
            }
        });

        $('.panel .tools .fa-times').click(function () {
            $(this).parents(".panel").parent().remove();
        });

        // tool tips
        $('.tooltips').tooltip();

        // popovers
        $('.popovers').popover();
    });

})(jQuery);

function SetActiveMenu(id) {
    var idlst = id.split('+');
    let parent = idlst[0].split(',');
    let menu = idlst[1];
    for (var i = 0; i < parent.length; i++) {
        $('#P_' + parent[i] + '').find('a:first').trigger('click');
        $($('#P_' + parent[i] + '').find('a:first')).css('color','#1FB5AD');
    }
    $($('#P_' + menu + '').find('a:first')).css('color', '#1FB5AD');
}

//function SetActiveMenu(id) {
//    $(".sidebar-menu li a").removeClass("active");
//    var lilist = document.getElementsByClassName('lilist');

//    for (var i = 0; i < lilist.length; i++) {
//        $('#P_' + lilist[i].id).removeClass('active');
//        $('#P_' + lilist[i].id).find('a:first').removeClass('active');
//    }

//    if (id.indexOf("+") > -1) {
//        var sublist = document.getElementsByClassName('Sublilist');
//        for (var i = 0; i < sublist.length; i++) {
//            $('#P_' + sublist[i].id).removeClass('active');
//            $('#P_' + sublist[i].id).find('a:first').removeClass('active');
//        }
//        var parts = [];
//        parts = id.split("+");
//        for (var i = 0; i < parts.length; i++) {
//            if (document.getElementById('P_' + parts[i]) != null) {
//                $('#P_' + parts[i]).removeClass('active');
//                $('#P_' + parts[i]).find('a:first').removeClass('active');

//                $('#P_' + parts[i]).addClass('active');
//                $('#P_' + parts[i]).find('a:first').addClass('active');
//            }
//        }
//    }
//    else {
//        $('#P_' + id).removeClass('active');
//        $('#P_' + id).find('a:first').removeClass('active');
//        $('#P_' + id).addClass('active');
//        $('#P_' + id).find('a:first').addClass('active');
//    }
//}

function Onsuccess() {
    var msg = document.getElementById('SuccessMessage').value;
    var errmsg = document.getElementById('ErrorMessage').value;
    if (msg != "" && msg != null) {
        notie.alert(1, msg, 2);
    }
    else if (errmsg != "" && errmsg != null) {
        notie.alert(3, 'Warning<br>' + errmsg + '', 2)
    }
}

function fnReset() {

    $('select option[value=""]').attr("selected", true);
    var ele = document.getElementsByClassName('form-control');
    $("input[type=hidden]").each(function () {
        $(this).val('');
    });

    var errorlab = document.getElementsByClassName('label1');

    for (var i = 0; i < errorlab.length; i++) {
        errorlab[i].textContent = "";
    }

    for (var i = 0; i < ele.length; i++) {

        if (ele[i].tagName == 'INPUT') {
            ele[i].value = '';
            if (ele[i].type == 'file') {
                ele[i].defaultValue = '';
            }
            if (ele[i].type == 'checkbox') {
                ele[i].checked = false;
            }
        }
        else if (ele[i].tagName == 'TEXTAREA') {
            $(ele[i]).empty();
            $(ele[i]).val('');
            if (typeof CKEDITOR != "undefined") {
                for (instance in CKEDITOR.instances) {
                    CKEDITOR.instances[instance].setData('');
                }
            }
        }
        else if (ele[i].tagName == 'LABEL') {
            $('#' + ele[i].id).empty();
            if (document.getElementById(ele[i].id)) {
                document.getElementById(ele[i].id).innerHTML = "";
            }
        }

        else if (ele[i].tagName == 'SELECT') {
            if (document.getElementById(ele[i].id)) {
                document.getElementById(ele[i].id).value = "";
            }
        }

        else if (ele[i].type == 'checkbox') {
            alert('Checkbox');
        }
        $('#myModal').modal('hide');
        $('#Modal').modal('hide');
        $("#images").empty();
        $("#DisplayImg").hide();
        $("#cnlImgIcon").hide();
        $("#DisplayImgicon").hide();
        $("#cnlImg").hide();
        if ($('#Image')) {
            $('#Image').attr('src', '../Content/images/notavailable.png');
            $('#divchange').css('display', 'none');
        }
    }

    var elecheck = document.getElementsByClassName('chkactive');
    for (var i = 0; i < elecheck.length; i++) {
        elecheck[i].checked = true;
    }
    var eleuncheck = document.getElementsByClassName('chkinactive');
    for (var i = 0; i < eleuncheck.length; i++) {
        eleuncheck[i].checked = false;
    }
}

function scrolltop() {
    $('html, body').animate({ scrollTop: 0 }, 800);
}



if (!window.location.href.toLowerCase().includes("home/index")) {
    $('#header_widget_list').remove();
}


function fnExportToExcel() {
    //var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var tab_text = "<table border='2px'><tr'>";
    var textRange; var j = 0;
    tab = document.getElementById('export-to-excel'); // id of table

    for (j = 0; j < tab.rows.length; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    debugger;
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else                 //other browser not tested on IE 11
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}