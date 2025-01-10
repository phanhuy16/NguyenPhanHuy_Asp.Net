// some scripts
// jquery ready start
$(document).ready(function () {
    //////////////////////// Prevent closing from click inside dropdown
    $(document).on('click', '.dropdown-menu', function (e) {
        e.stopPropagation();
    });

    //////////////////////// plus minus input
    $('#button-plus').on('click', function () {
        let input = $(this).closest('.input-spinner').find('#quantity');
        let value = parseInt(input.val());
        input.val(value + 1);
    })

    $('#button-minus').on('click', function () {
        let input = $(this).closest('.input-spinner').find('#quantity');
        let value = parseInt(input.val());
        if (value > 1) {
            input.val(value - 1);
        } else {
            input.val(1);
        }
    })

    $('body').on('click', '.button-plus', function () {
        let id = $(this).data('id');
        let input = $('#quantity_' + id);
        let value = parseInt(input.val());
        input.val(value + 1);
    })

    $('body').on('click', '.button-minus', function () {
        let id = $(this).data('id');
        let input = $('#quantity_' + id);
        let value = parseInt(input.val());
        if (value > 1) {
            input.val(value - 1);
        } else {
            input.val(1);
        }
    })

    //////////////////////// Bootstrap tooltip
    if ($('[data-toggle="tooltip"]').length > 0) {  // check if element exists
        $('[data-toggle="tooltip"]').tooltip()
    } // end if

    //////////////////////// Add to cart
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        let id = $(this).data('id');
        let qty = 1;
        let quantity = $('#quantity').val();
        if (quantity != "") {
            qty = parseInt(quantity);
        }

        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: {
                id: id,
                qty: qty
            },
            success: function (result) {
                if (result.success) {
                    $('#checkout-items').html(result.count);
                    toastr.success(result.message);
                } else {
                    toastr.error(result.message);
                }
            }
        })
    })

    //////////////////////// Remove cart item
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        let id = $(this).data('id');
        let conf = confirm('Bạn có chắc chắn muốn xoá nó khỏi giỏ hàng không?');

        if (conf) {
            $.ajax({
                url: '/Cart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (result) {
                    if (result.success) {
                        $('#checkout-items').html(result.count);
                        $('#trow_' + id).remove();
                        Loading();
                        toastr.success(result.message);
                    } else {
                        toastr.error(result.message);
                    }
                }
            })
        }
    })

    //////////////////////// Remove all cart item
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();

        let conf = confirm('Bạn có chắc chắn muốn xoá hết sản phẩm khỏi giỏ hàng không?');

        if (conf) {
            deleteAll();
        }
    })

    //////////////////////// Update cart item
    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        let id = $(this).data('id');
        let qty = $('#quantity_' + id).val();

        Update(id, qty);
    })

    showCount();
});

function showCount() {
    $.ajax({
        url: '/Cart/ShowCount',
        type: 'GET',
        success: function (result) {
            if (result.success) {
                $('#checkout-items').html(result.count);
            }
        }
    })
}

function deleteAll() {
    $.ajax({
        url: '/Cart/DeleteAll',
        type: 'POST',
        success: function (result) {
            if (result.success) {
                toastr.success(result.message);
                Loading();
            } else {
                toastr.error(result.message);
            }
        }
    })
}

function Update(id, qty) {
    $.ajax({
        url: '/Cart/Update',
        type: 'POST',
        data: {
            id: id,
            qty: qty
        },
        success: function (result) {
            if (result.success) {
                toastr.success(result.message);
                Loading();
            } else {
                toastr.error(result.message);
            }
        }
    })
}

function Loading() {
    $.ajax({
        url: '/Cart/PartialCartItem',
        type: 'GET',
        success: function (result) {
            $('#loading-data').html(result);
        },
    });
}

// jquery end