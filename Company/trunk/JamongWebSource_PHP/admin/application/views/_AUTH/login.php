<div class="login-box">
    <div class="login-logo">
        <a href="<?=site_url('/home/index')?>"><b>PROJECT A</b></a>
    </div>
    <div class="login-box-body">
        <form action="<?=site_url('/auth/login')?>" method="post">
            <div class="form-group has-feedback">
                <input type="email" class="form-control" placeholder="Email" name="email" id="email"/>
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <input type="password" class="form-control" placeholder="Password" name="password"/>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>
            <div class="row">
                <div class="col-xs-12">
                </div>
                <div class="col-xs-12">
                    <button type="submit" class="btn btn-default btn-block btn-flat" style="background-color: #222d32;">
                        <span id="localize_25" style="color: white;"><?php echo $this->lang->line('로그인'); ?></span></button>
                </div>
            </div>
        </form>
    </div>
</div>

