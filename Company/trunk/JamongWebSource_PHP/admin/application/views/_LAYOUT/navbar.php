<aside class="main-sidebar">
    <section class="sidebar">
        <ul class="sidebar-menu">
            <li class="header">
                MANAGEMENT
            </li>
            <li><a href="<?= site_url('home/index') ?>"><i class="fa fa-database"></i> <span>DASHBOARD</span></a></li>
            <li>
                <a href="<?= site_url("user/index") ?>">
                    <i class="fa fa-users"></i> <span>USER</span>
                </a>
            </li>
            <li>
                <a href="<?= site_url('machine/index') ?>">
                    <i class="fa fa-car"></i> <span>MACHINE</span>
                </a>
            </li>
            <li>
                <a href="<?= site_url('content/index') ?>">
                    <i class="fa fa-camera"></i> <span>CONTENT</span>
                </a>
            </li>
            <li>
                <a href="<?= site_url('machine_content/index') ?>">
                    <i class="fa fa-download"></i> <span>MACHINE CONTENT</span>
                </a>
            </li>
            <li>
                <a href="<?= site_url('logs/index') ?>">
                    <i class="fa fa-book"></i> <span>LOGS</span>
                </a>
            </li>
            <li>
                <a href="<?= site_url('balance/index') ?>">
                    <i class="fa fa-money"></i> <span>BALANCE</span>
                </a>
            </li>
        </ul>
    </section>
</aside>

<script src="https://code.jquery.com/jquery-1.10.2.js"></script>
<script>
    // FIXME: 언젠가 헤더로 추가하는 것이 좋을지도 모른다
    function post(URL, PARAMS) {
        var temp = document.createElement("form");
        temp.action = URL;
        temp.method = "POST";
        temp.style.display = "none";
        for(var x in PARAMS) {
            var opt = document.createElement("textarea");
            opt.name = x;
            opt.value = PARAMS[x];
            temp.appendChild(opt);
        }
        document.body.appendChild(temp);
        temp.submit();
        return temp;
    }

    $('#navbar_chart_button').click(function (){
        $.ajax({
            url: '/admin/chart/get_channel_num',
            dataType: 'json',
            type: 'post',
            data: {
                user_num: <?php echo json_encode($this->session->userdata('userid')); ?>
            },
            success: function (data){
                post("/admin/chart/detail", {
                    channelId: data
                });
            },
            error: function (err){
                alert("ERROR: " + JSON.stringify(err));
            }
        });
    });
</script>