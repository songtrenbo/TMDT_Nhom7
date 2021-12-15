$('.modal-Feedback').click(function () {
    $('#hidFeedback').val($(this).data('id'));
})

function Feedback(maHoaDon) {
    var maSP = $('#hidFeedback').val();
    var rating = $('input[name="rating"]:checked').val();
    var content = $('#FeedbackContent').val();
    $.ajax({
        type: "POST",
        url: "/KhachHang/Feedback",
        data: {
            maSP: maSP,
            maHoaDon: maHoaDon,
            rating: rating,
            content: content,
        },
        success: function () {
            FeedbackSuccess();
            location.reload();
        }
    })
}

function FeedbackSuccess() {
    Swal.fire({
        icon: 'success',
        title: 'Lưu đánh giá thành công!',
        showConfirmButton: false,
        timer: 1500
    })
}