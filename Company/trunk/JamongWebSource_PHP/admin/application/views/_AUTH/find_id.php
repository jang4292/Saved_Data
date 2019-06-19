<div class="login-box">
    <div class="login-logo">
        <a href="<?=site_url('/home/index')?>"><b>VR HUB</b></a>

    </div>

    <form action="<?=site_url('/auth/submit_find_id')?>" method="post">

        <div class="btn-group btn-group-justified">
            <a href="<?= site_url('/auth/login') ?>" class="btn btn-default" >
                <span id="localize_7"><?php echo $this->lang->line('로그인'); ?></span></a>
            <a href="<?= site_url('/auth/join') ?>" class="btn btn-default" >
                <span id="localize_8"><?php echo $this->lang->line('회원가입'); ?></span></a>
        </div>

        <div class="login-box-body">

            <h1><p class="login-box-msg" id="localize_9"><?php echo $this->lang->line('이메일 찾기'); ?></p></h1>


            <div class="form-group has-feedback">
                <input type="text" class="form-control" placeholder="Name" name="nickname"/>
                <span class="glyphicon glyphicon-user form-control-feedback"></span>
                <br>
                <ul>
                    <li>
                        <?php echo $this->lang->line('찾고자 하는 이메일의 닉네임을 입력해주세요.') ?>
                    </li>
                </ul>
            </div>
            <div class="row">
                <div class="col-xs-12">
                </div>
                <div class="col-xs-12">
                    <button type="submit_find_id" class="btn btn-primary btn-block btn-flat">
                        <span id="localize_10"><?php echo $this->lang->line('이메일 찾기'); ?></span> </button>
                </div>

            </div>
    </form>
</div>
</div>