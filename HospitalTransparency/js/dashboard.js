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

$(document).on('click', '.event-close', function () {
    $(this).closest("li").remove();
    return false;
});

$('.progress-stat-bar li').each(function () {
    $(this).find('.progress-stat-percent').animate({
        height: $(this).attr('data-percent')
    }, 1000);
});

$('.todo-check label').click(function () {
    $(this).parents('li').children('.todo-title').toggleClass('line-through');
});


$(document).on('click', '.todo-remove', function () {
    $(this).closest("li").remove();
    return false;
});


$('.stat-tab .stat-btn').click(function () {

    $(this).addClass('active');
    $(this).siblings('.btn').removeClass('active');

});

//$('select.styled').customSelect();
$("#sortable-todo").sortable();

