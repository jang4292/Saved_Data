<footer class="main-footer">
    <div class="pull-right hidden-xs">
       <!--<b>Management</b>-->
    </div>
    <strong>Copyright <a href="/admin/easteregg/index"><img src="http://www.pokemon-go-jyudenki.com/wp-content/uploads/2016/08/icon_130460_256.png" width="10" height="10"></a>
 <a href="http://www.jamong360.com/" target="_blank">JAMONG</a>.</strong> All rights reserved.
</footer>

</div>
<script src="/static/lib/admin/plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
<script src="/static/js/angular.min.js" type="text/javascript"></script>
<script src="/static/lib/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

<script src="<?= site_url() ?>static/lib/ngTable.min.js"></script>
<script src="/static/lib/admin/plugins/datatables/jquery.dataTables.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/plugins/datatables/dataTables.bootstrap.min.js" type="text/javascript"></script>

<script src="/static/lib/admin/plugins/slimScroll/jquery.slimscroll.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/plugins/fastclick/fastclick.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/plugins/select2/select2.full.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/js/app.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/js/demo.js" type="text/javascript"></script>
<script src="/static/js/ajaxBody.js"></script>
<script src="<?= site_url('static/js/common.js') ?>"></script>

<?php
$total_url = $_SERVER['PHP_SELF'];
$arr_splitted_url = explode('/', $total_url);

$ctrl_name = $arr_splitted_url[count($arr_splitted_url) - 2];
$view_name = $arr_splitted_url[count($arr_splitted_url) - 1];
$filename = "";

if ($ctrl_name == 'index.php') {
    $filename = 'static/js/' . strtolower($view_name) . '/index.js';
} else {
    $filename = 'static/js/' . strtolower($ctrl_name) . '/' . strtolower($view_name) . '.js';
}

if (file_exists($filename)) {
    ?>
    <script src="/admin/<?php echo $filename; ?>"></script>
    <?php
}


if (strpos($filename, 'create') || strpos($filename, 'submit') || strpos($filename, 'update')) {
    ?>
    <?php
}
?>
<!--
<script>
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#imgpre').val(input.value.replace(/C:\\fakepath\\/i, ''));
                $('#imgpre').attr('src', e.target.result);
                $('#imgpre02').attr('src', e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imgInp").change(function(){
        readURL(this);
    });
</script>
-->
</body>
</html>
