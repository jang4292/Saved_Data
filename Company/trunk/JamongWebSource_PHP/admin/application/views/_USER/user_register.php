<div ng-controller="DetailCtrl">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>사용자 등록</h1>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12 jamong-pannel">
                    <div class="box box-default">
                        <form class="form-horizontal" style="padding-top: 20px; padding-left: 300px" action="<?= site_url('/auth/submit_join') ?>"
                              method="post">
                            <div class="form-group">
                                <label class="col-sm-1 control-label">이름</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" placeholder="이름" name="user_name"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-1 control-label" >이메일</label>
                                <div class="col-sm-6">
                                    <input type="email" class="form-control" placeholder="이메일" name="user_id"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-1 control-label" >비밀번호</label>
                                <div class="col-sm-6">
                                    <input type="password" class="form-control" placeholder="비밀번호" name="password"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-1 control-label">비밀번호 확인</label>
                                <div class="col-sm-6">
                                    <input type="password" class="form-control" placeholder="비밀번호 확인" name="password_confirm"/>
                                </div>
                            </div>
                            <!--
                            <div class="form-group">
                                <label class="col-sm-1 control-label">권한부여</label>
                                <div class="col-sm-6">
                                    <input type="checkbox" name="permission[]" value="1" >총판
                                    &nbsp;&nbsp;
                                    <input type="checkbox" name="permission[]" value="2">영상
                                    &nbsp;&nbsp;
                                    <input type="checkbox" name="permission[]" value="4">기기
                                    &nbsp;&nbsp;
                                    <input type="checkbox" name="permission[]" value="8">점주
                                    &nbsp;&nbsp;
                                    <input type="checkbox" name="permission[]" onclick=chkBox(this.checked)>복합
                                </div>
                            </div>
                            -->
                            <div class="form-group" align="right" style="padding-bottom: 20px; padding-right: 590px">
                                <button class="btn btn-primary" type="submit_join">등록하기<i class="fa fa-check spaceLeft"></i></button>
                            </div>
                    </div>
                    </form>
                </div>
            </div>
    </div>
    </section>
</div>
</div>
<!--
<script type="text/javascript" src="user_register.php"></script>
-->