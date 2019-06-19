<script src="/static/lib/admin/plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
<script src="/static/lib/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
<script src="/static/lib/admin/plugins/iCheck/icheck.min.js" type="text/javascript"></script>
<script>
    /**
     * 단축url 키등록
     */
    function load() {
        gapi.client.setApiKey('AIzaSyDwWcpxM-X9fmtbspybET9QaAUwwC2XNJ0');
        gapi.client.load('urlshortener', 'v1',function(){});
    }

    $('.dj-test').click(function() {
        var longURL = $('#longURL').val();
        var request = gapi.client.urlshortener.url.insert({
            'resource' : {
                'longUrl' : "https://s3-ap-northeast-1.amazonaws.com/vr-hub.movie/original/37_2016-05-11_14:49:11_test_video.mp4"
            }
        });
        request.execute(function(response) {
            if (response.id != null) {
                console.log(response.id);
                //$('#shortURL').val(response.id);
            } else {
                alert("error: creating short url");
            }
        });
    });
</script>
<script src="https://apis.google.com/js/client.js?onload=load"></script>
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


if (strpos($filename, 'create') || strpos($filename, 'update')) {
    ?>
    <script src="/static/lib/smarteditor/js/HuskyEZCreator.js"></script>
    <?php
}
?>
</body>
</html>
