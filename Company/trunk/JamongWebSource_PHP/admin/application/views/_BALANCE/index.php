<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<?php
$array_sum_customers = array(
    array('일자', '탑승객수')
);
foreach($array_balance as $value){
    $value_array = array($value->period, intval($value->sum_customers));
    array_push($array_sum_customers, $value_array);
}

$array_customers_price = array(
    array('일자', '총계')
);
foreach($array_balance as $value){
    $value_array = array($value->period, intval($value->customers_price));
    array_push($array_customers_price, $value_array);
}
?>
<div class="content-wrapper">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502">정산</h1>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <div class="box-body table-responsive table-container">
                        <!--<div class="col-lg-12 col-md-12 col-sm-12"><div id="graph_sum_customers" class="text-center">그래프 자리</div></div>-->
                        <div class="col-lg-12 col-md-12 col-sm-12"><div id="graph_customers_price" class="text-center">그래프 자리</div></div>
                        <table id="balance_table" class="table table-bordered table-hover">
                            <thead>
                                <th>
                                    기간
                                </th>
                                <th>
                                    탑승객수
                                </th>
                                <th>
                                    총계
                                </th>
                            </thead>
                            <tbody>
                            
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>
<script src="//www.gstatic.com/charts/loader.js"></script>
<script src="//www.google.com/jsapi"></script>
<script src="https://code.jquery.com/jquery-1.10.2.js"></script>
<script>
    var array_balance = <?php echo json_encode($array_balance); ?>;
    var array_sum_customers = <?php echo json_encode($array_sum_customers); ?>;
    var array_customers_price = <?php echo json_encode($array_customers_price); ?>;
</script>