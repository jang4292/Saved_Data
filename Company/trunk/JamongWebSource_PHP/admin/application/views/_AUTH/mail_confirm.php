<div class="login-box">
    <div class="login-logo">
        <a href="<?=site_url('/home/index')?>"><b>VR HUB</b></a>

    </div>



    <div class="btn-group btn-group-justified">
        <a href="<?= site_url('/auth/login') ?>" class="btn btn-default">
            <span id="localize_28"><?php echo $this->lang->line('로그인'); ?></span> </a>
        <a href="<?= site_url('/auth/join') ?>" class="btn btn-default">
            <span id="localize_29"><?php echo $this->lang->line('회원가입'); ?></span></a>
    </div>

    <div class="login-box-body">

        <h1><p class="login-box-msg" id="localize_30"><?php echo $this->lang->line('이메일 인증하기'); ?></p></h1>

        <form action="<?=site_url('/auth/send_mail_join')?>" method="post">
            <div class="form-group has-feedback">
                <input type="email" class="form-control" placeholder="Email" name="email"/>
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
            </div>
            <div class="row">
                <div class="col-xs-12">
                </div>
                <div class="col-xs-12">
                    <button type="send_mail_join" class="btn btn-primary btn-block btn-flat" >
                        <span id="localize_31"><?php echo $this->lang->line('이메일 보내기'); ?></span></button>
                </div>
                <div class="col-xs-12 btn-join" style="margin-top: 10px">

                </div>
            </div>
        </form>
    </div>
</div>