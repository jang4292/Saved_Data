<div class="login-box">
    <div class="login-logo">
        <a href="<?=site_url('/home/index')?>"><b>VR HUB</b></a>

    </div>

    <form action="<?=site_url('/auth/submit_find_password')?>" method="post">

        <div class="btn-group btn-group-justified">
            <a href="<?= site_url('/auth/login') ?>" class="btn btn-default"><span id="localize_11"><?php echo $this->lang->line('로그인'); ?></span> </a>
            <a href="<?= site_url('/auth/join') ?>" class="btn btn-default"><span id="localize_12"><?php echo $this->lang->line('회원가입'); ?></span> </a>
        </div>

        <div class="login-box-body">

            <h1><p class="login-box-msg" id="localize_13"><?php echo $this->lang->line('비밀번호 찾기'); ?></p></h1>


            <div class="form-group has-feedback">
                <input type="email" class="form-control" placeholder="Email" name="email"/>
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
            </div>
            <div class="row">
                <div class="col-xs-12">
                </div>
                <div class="col-xs-12">
                    <button type="submit_find_password" class="btn btn-primary btn-block btn-flat" >
                        <span id="localize_14"><?php echo $this->lang->line('비밀번호 재설정'); ?></span></button>
                </div>

            </div>
            <br/>
            <ul>
                <li id="localize_15"><?php echo $this->lang->line('가입시 등록했던 이메일로 임시 비밀번호를 보내드립니다.'); ?></li>
                <li id="localize_16"><?php echo $this->lang->line('메일 발송 후 1시간 이내로 발송된 비밀번호로 로그인을 해야 변경됩니다.'); ?></li>
            </ul>
    </form>
</div>
</div>