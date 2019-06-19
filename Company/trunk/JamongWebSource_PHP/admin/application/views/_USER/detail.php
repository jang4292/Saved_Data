<input id="jamong-userId" type="hidden" value="<?php echo $this->session->userdata('userid') ?>"
       xmlns="http://www.w3.org/1999/html">
<input id="jamong-user-id" type="hidden" value="<?php echo $item->index; ?>" >
<div ng-controller="DetailCtrl">
    <div class="content-wrapper">
        <section class="content-header">
            <h1 id="localize_442"><?php echo $this->lang->line('사용자'); ?></h1>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12 jamong-pannel">
                    <div class="box box-default">
                        <form class="form-horizontal" action="<?= site_url('/home/change_password/') ?>"
                              method="post" enctype="multipart/form-data">
                            <input type="hidden" name="userId" value="<?php if ($item != null) echo $item->uid ?>">
                            <div class="box-header with-border">
                                <h3 class="box-title" id="localize_381"><?php echo $this->lang->line('기본정보'); ?></h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_383"><?php echo $this->lang->line('이름'); ?></label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo $item->user_name ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_384"><?php echo $this->lang->line('이메일'); ?></label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo $item->user_id ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_388"><?php echo $this->lang->line('비밀번호'); ?></label>
                                    <div class="col-sm-9 sg-item-content">
                                        <input type="password" class="jamong-password-input form-control my-colorpicker1"
                                               name="password" ng-model="user.password"/>
                                        <button type="submit" class="btn btn-default jamong-password-change-submit" style="background-color: #222d32;"><span id="localize_389" style="color: white;"><?php echo $this->lang->line('비밀번호 변경'); ?></span></button>
                                    </div>
                                </div>
                        </form>
                        <div class="form-group">
                            <label for="title" class="col-sm-3 control-label" style="padding-top: 25px">권한변경</label>
                            <div class="col-sm-9 sg-item-content">
                                <form
                                    action="<?= site_url('/api/user/update_user_auth?userId=' . $item->index) ?>"
                                    style="margin-top: 10px" method="post" selected>
                                    <?php if ($item->user_auth == 1 || $item->user_auth == 3 || $item->user_auth == 5 || $item->user_auth == 7 || $item->user_auth == 9 || $item->user_auth == 11 || $item->user_auth == 13 || $item->user_auth == 15) { ?>
                                        <input type="checkbox" name="permission[]" value="1" checked="checked">총판
                                    <?php } else {?>
                                        <input type="checkbox" name="permission[]" value="1" >총판
                                    <?php } ?>
                                    <?php if ($item->user_auth == 2 || $item->user_auth == 3 || $item->user_auth == 6 || $item->user_auth == 7 ||$item->user_auth == 10 ||$item->user_auth == 11 || $item->user_auth == 14 ||$item->user_auth == 15) { ?>
                                        <input type="checkbox" name="permission[]" value="2" checked="checked">영상
                                    <?php } else {?>
                                        <input type="checkbox" name="permission[]" value="2">영상
                                    <?php } ?>
                                    <?php if ($item->user_auth == 4 || $item->user_auth == 5 || $item->user_auth == 6 || $item->user_auth == 7 || $item->user_auth == 12 || $item->user_auth == 13 || $item->user_auth == 14 || $item->user_auth == 15) { ?>
                                        <input type="checkbox" name="permission[]" value="4" checked="checked">기기
                                    <?php } else {?>
                                        <input type="checkbox" name="permission[]" value="4">기기
                                    <?php } ?>
                                    <?php if ($item->user_auth == 8 || $item->user_auth == 9 || $item->user_auth == 10 || $item->user_auth == 11 || $item->user_auth == 12 || $item->user_auth == 13 || $item->user_auth == 14 || $item->user_auth == 15) { ?>
                                        <input type="checkbox" name="permission[]" value="8" checked="checked">점주
                                    <?php } else {?>
                                        <input type="checkbox" name="permission[]" value="8">점주
                                    <?php } ?>
                                    <?php if ($item->user_auth == 15) { ?>
                                        <input type="checkbox" name="permission[]" checked="checked" onclick=chkBox(this.checked)>복합
                                    <?php } else {?>
                                        <input type="checkbox" name="permission[]" onclick=chkBox(this.checked)>복합
                                    <?php } ?>
                                    <input type="submit" style="background-color: #222d32; color: white" class="btn" value="권한부여"/>
                                </form>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12 jamong-pannel">
                    <div class="box box-default">
                        <form class="form-horizontal" enctype="multipart/form-data">
                            <input type="hidden" name="userId" value="<?php if ($item != null) echo $item->uid ?>">
                            <div class="box-header with-border">
                                <h3 class="box-title" id="localize_381">특수정보</h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_382">고유식별번호</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo $item->uid ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_385">최고관리자 여부</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->user_auth == 0 ? '<label style="color: red;">최고관리자가 맞습니다.</label>' : '<label>최고관리자가 아닙니다.</label>'); ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_385">총판관리자 여부</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->user_auth & 1 ? '<label style="color: red;">총판관리자가 맞습니다.</label>' : '<label>총판관리자가 아닙니다.</label>'); ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_385">영상관리자 여부</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->user_auth & 2 ? '<label style="color: red;">영상관리자가 맞습니다.</label>' : '<label>영상관리자가 아닙니다.</label>'); ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_385">기기관리자 여부</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->user_auth & 4 ? '<label style="color: red;">기기관리자가 맞습니다.</label>' : '<label>기기관리자가 아닙니다.</label>'); ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_385">지점관리자 여부</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->user_auth & 8 ? '<label style="color: red;">지점관리자가 맞습니다.</label>' : '<label>지점관리자가 아닙니다.</label>'); ?>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="title" class="col-sm-3 control-label" id="localize_387">취득수수료</label>
                                    <div class="col-sm-9 sg-item-content">
                                        <?php echo ($item->tax_price * 100).' %' ?>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </section>
    </div>
</div>
<script type="text/JavaScript">
    function chkBox(bool) { // 전체선택/해제
        var obj = document.getElementsByName("permission[]");
        for (var i=0; i<obj.length; i++) obj[i].checked = bool;
    }

</script>