<div class="login-box">
    <div class="login-logo">
        <a href="<?=site_url('/home/index')?>"><b>VR HUB</b></a>

    </div>

    <form action="<?=site_url('/auth/submit_join')?>" method="post">
        <div class="btn-group btn-group-justified">
            <a href="<?= site_url('/auth/login') ?>" class="btn btn-default">
                <span id="localize_17"><?php echo $this->lang->line('로그인'); ?></span></a>
            <a href="<?= site_url('/auth/join') ?>" class="btn btn-default">
               <span id="localize_18"><?php echo $this->lang->line('회원가입'); ?></span></a>
        </div>
        
        <div class="login-box-body">

            <h1><p class="login-box-msg">SIGN UP</p></h1>

            <div class="form-group has-feedback">
                <input type="text" class="form-control" placeholder="Name" name="nickname"/>
                <span class="glyphicon glyphicon-user form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <input type="email" class="form-control" placeholder="Email" name="email"/>
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <input type="password" class="form-control" placeholder="Password" name="password"/>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <input type="password" class="form-control" placeholder="PasswordConfirm" name="password_confirm"/>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>

            <table width="100%">
                <tr>
                    <td class="text-center">
                        <div class="form-group radio-input-male">
                            <input id="male" type="radio" name="gender" value="Male">
                            <label for="male" id="localize_19"><?php echo $this->lang->line('남자'); ?></label>
                        </div>
                    </td>
                    <td class="text-center">
                        <div class="form-group radio-input-female">
                            <input id="female" type="radio" name="gender" value="Female">
                            <label for="female" id="localize_20"><?php echo $this->lang->line('여자'); ?></label>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="form-group">
                <input name="ch-agree" class="check-box" type="checkbox" required><span id="localize_21"><?php echo $this->lang->line('이용약관과 개인정보취급방침을 모두 읽고, 동의합니다.'); ?></span>
            </div>
            <div class="row">
                <div class="col-xs-12">
                </div>
                <div class="col-xs-12">
                    <button type="submit_join" class="btn btn-primary btn-block btn-flat" >
                        <span id="localize_22"><?php echo $this->lang->line('회원가입'); ?></span> </button>
                </div>
                
            </div>
    </form>
</div>
</div>

