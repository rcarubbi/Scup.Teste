/*
 *
 *   INSPINIA - Responsive Admin Theme
 *   version 2.0
 *
 */
(function () {
    var $private = {};

    $(function () {
        $private.adjustBody();
        $private.activateSideMenu();
        $private.assinarEventos();
        $private.initializeSlimControl();
        $private.initializeSlimControlSmallChat();
        $private.initializeTooltip();
        $private.fixBootstrapBackdropIssue();
        $private.fixHeight();
        $private.initializePopover();

        if ($private.hasLocalStorageSupport) {
            $private.restoreLocalStorageValues();
        }

      
    });

    $private.adjustBody = function () {
        // Add body-small class if window less than 768px
        if ($(window).width() < 769) {
            $('body').addClass('body-small')
        } else {
            $('body').removeClass('body-small')
        }
    };

    $private.activateSideMenu = function () {
        // MetsiMenu
        $('#side-menu').metisMenu();
    };

    $private.assinarEventos = function () {
        $('.collapse-link').on("click", $private.collapseIbox);
        $('.close-link').on("click", $private.closeIbox);
        $('.close-canvas-menu').on("click", $private.closeCanvasMenu);
        $('.right-sidebar-toggle').on("click", $private.toggleSideBar);
        $('.open-small-chat').on("click", $private.toggleSmallChat);
        $('.check-link').on("click", $private.chekcSmallTodo);
        $('.navbar-minimalize').on("click", $private.minimalizeNavbar);
        $(window).on("load", $private.fixSidebar);
        $(window).on("scroll", $private.moveRightSidebarTopAfterScroll);
        $(document).bind("load resize scroll", $private.checkFixHeightNeeded);
        $(window).bind("resize", $private.minimalizeMenu);

    };

    $private.collapseIbox = function () {
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        var content = ibox.find('div.ibox-content');
        content.slideToggle(200);
        button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
        ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            ibox.resize();
            ibox.find('[id^=map-]').resize();
        }, 50);
    };

    $private.closeIbox = function () {
        var content = $(this).closest('div.ibox');
        content.remove();
    };

    $private.closeCanvasMenu = function () {
        $("body").toggleClass("mini-navbar");
        $private.smoothlyMenu();
    };

    $private.toggleSideBar = function () {
        $('#right-sidebar').toggleClass('sidebar-open');
    };

    $private.initializeSlimControl = function () {
        $('.sidebar-container').slimScroll({
            height: '100%',
            railOpacity: 0.4,
            wheelStep: 10
        });

        $('.full-height-scroll').slimscroll({
            height: '100%'
        })
    };

    $private.toggleSmallChat = function () {
        $(this).children().toggleClass('fa-comments').toggleClass('fa-remove');
        $('.small-chat-box').toggleClass('active');
    };

    $private.initializeSlimControlSmallChat = function () {
        // Initialize slimscroll for small chat
        $('.small-chat-box .content').slimScroll({
            height: '234px',
            railOpacity: 0.4
        });

    };

    $private.chekcSmallTodo = function () {
        var button = $(this).find('i');
        var label = $(this).next('span');
        button.toggleClass('fa-check-square').toggleClass('fa-square-o');
        label.toggleClass('todo-completed');
        return false;
    };

    $private.minimalizeNavbar = function () {
        $("body").toggleClass("mini-navbar");
        $private.smoothlyMenu();
    };

    $private.initializeTooltip = function () {
        $('.tooltip-demo').tooltip({
            selector: "[data-toggle=tooltip]",
            container: "body"
        });
    };

    $private.fixBootstrapBackdropIssue = function () {
        // Move modal to body
        // Fix Bootstrap backdrop issu with animation.css
        $('.modal').appendTo("body");

    };

    $private.fixHeight = function () {
        var heightWithoutNavbar = $("body > #wrapper").height() - 61,
            navbarHeigh = $('nav.navbar-default').height(),
            wrapperHeigh = $('#page-wrapper').height();

        $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

        if (navbarHeigh > wrapperHeigh) {
            $('#page-wrapper').css("min-height", navbarHeigh + "px");
        }

        if (navbarHeigh < wrapperHeigh) {
            $('#page-wrapper').css("min-height", $(window).height() + "px");
        }
    };

    $private.fixSidebar = function () {
        if ($("body").hasClass('fixed-sidebar')) {
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }
    };

    $private.moveRightSidebarTopAfterScroll = function () {
        if ($(window).scrollTop() > 0 && !$('body').hasClass('fixed-nav')) {
            $('#right-sidebar').addClass('sidebar-top');
        } else {
            $('#right-sidebar').removeClass('sidebar-top');
        }
    };

    $private.checkFixHeightNeeded = function () {
        if (!$("body").hasClass('body-small')) {
            $private.fixHeight();
        }
    };

    $private.initializePopover = function () {
        $("[data-toggle=popover]").popover();
    };

    $private.minimalizeMenu = function () {
        // Minimalize menu when screen is less than 768px
        if ($(this).width() < 769) {
            $('body').addClass('body-small')
        } else {
            $('body').removeClass('body-small')
        }
    };

    $private.hasLocalStorageSupport = function () {
        // check if browser support HTML5 local storage
        return (('localStorage' in window) && window['localStorage'] !== null)
    }

    $private.restoreLocalStorageValues = function () {
        var collapse = localStorage.getItem("collapse_menu"),
         fixedsidebar = localStorage.getItem("fixedsidebar"),
         fixednavbar = localStorage.getItem("fixednavbar"),
         boxedlayout = localStorage.getItem("boxedlayout"),
         fixedfooter = localStorage.getItem("fixedfooter"),
         body = $('body');

        if (fixedsidebar == 'on') {
            body.addClass('fixed-sidebar');
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }

        if (collapse == 'on') {
            if (body.hasClass('fixed-sidebar')) {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }
            } else {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }

            }
        }

        if (fixednavbar == 'on') {
            $(".navbar-static-top").removeClass('navbar-static-top').addClass('navbar-fixed-top');
            body.addClass('fixed-nav');
        }

        if (boxedlayout == 'on') {
            body.addClass('boxed-layout');
        }

        if (fixedfooter == 'on') {
            $(".footer").addClass('fixed');
        }
    }

    $private.smoothlyMenu = function () {
        if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
            // Hide menu in order to smoothly turn on when maximize menu
            $('#side-menu').hide();
            // For smoothly turn on menu
            setTimeout(
                function () {
                    $('#side-menu').fadeIn(500);
                }, 100);
        } else if ($('body').hasClass('fixed-sidebar')) {
            $('#side-menu').hide();
            setTimeout(
                function () {
                    $('#side-menu').fadeIn(500);
                }, 300);
        } else {
            // Remove all inline style from jquery fadeIn function to reset menu state
            $('#side-menu').removeAttr('style');
        }
    }

    $private.winMove = function () {
        // Dragable panels
        var element = "[class*=col]";
        var handle = ".ibox-title";
        var connect = "[class*=col]";
        $(element).sortable(
            {
                handle: handle,
                connectWith: connect,
                tolerance: 'pointer',
                forcePlaceholderSize: true,
                opacity: 0.8
            })
            .disableSelection();
    }

    $private.animationHover = function (element, animation) {
        element = $(element);
        element.hover(
            function () {
                element.addClass('animated ' + animation);
            },
            function () {
                //wait for animation to finish before removing classes
                window.setTimeout(function () {
                    element.removeClass('animated ' + animation);
                }, 2000);
            });
    };

  
}());



















