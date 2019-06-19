<div class="content-wrapper">
    <section class="content-header">
        <h1 id="localize_516"><?php echo $this->lang->line('유저'); ?></h1>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                    <form class="form-horizontal" enctype="multipart/form-data"
                          action="<?= site_url('/user/submit_update') ?>" method="post" id="frm">
                        <input type="hidden" id="sg-create-userid" name="user" value="<?php echo $item->_userid ?>">

                        <div class="box-body">
                            <div class="form-group">
                                <label for="title" class="col-sm-1 control-label"><?php echo $this->lang->line('아이디');?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->_userid ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="localize_517" for="summary" class="col-sm-1 control-label"><?php echo $this->lang->line('이름'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->username ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="localize_518" for="summary" class="col-sm-1 control-label"><?php echo $this->lang->line('메일'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->email ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="localize_519" for="summary" class="col-sm-1 control-label"><?php echo $this->lang->line('프로필(URL)'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->profile_url ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="localize_520" for="summary" class="col-sm-1 control-label"><?php echo $this->lang->line('관리자'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php
                                    if ($item->is_admin) {
                                        ?>
                                        O
                                        <?php
                                    } else {
                                        ?>
                                        X
                                        <?php
                                    }
                                    ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_521"><?php echo $this->lang->line('로그인'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->logined ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_522"><?php echo $this->lang->line('수정일'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->updated ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_523"><?php echo $this->lang->line('생성일'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <?php echo $item->created ?>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_524"><?php echo $this->lang->line('페이스북'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <input type="text" class="form-control" name="facebook" id="facebook"
                                           value="<?php if (isset($item->facebook) && strlen($item->facebook) > 0) echo $item->facebook; ?>"
                                           placeholder="http://facebook.com/dongjin20"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_525"><?php echo $this->lang->line('인스타그램'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <input type="text" class="form-control" name="instagram" id="instagram"
                                           value="<?php if (isset($item->instagram) && strlen($item->instagram) > 0) echo $item->instagram; ?>"
                                           placeholder="http://instagram.com/heydj_"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_526"><?php echo $this->lang->line('블로그'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <input type="text" class="form-control" name="blog" id="blog"
                                           value="<?php if (isset($item->blog) && strlen($item->blog) > 0) echo $item->blog; ?>"
                                           placeholder="http://blog.naver.com/kdongj20"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_527"><?php echo $this->lang->line('핀터레스트'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <input type="text" class="form-control" name="pinterest" id="pinterest"
                                           value="<?php if (isset($item->pinterest) && strlen($item->pinterest) > 0) echo $item->pinterest; ?>"
                                           placeholder="http://pinterest.com/dongjin20"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="summary" class="col-sm-1 control-label" id="localize_528"><?php echo $this->lang->line('주소'); ?></label>

                                <div class="col-sm-11 sg-item-content">
                                    <input type="text" class="form-control" name="address" id="address" id="localize_529"
                                           value="<?php if (isset($item->address) && strlen($item->address) > 0) echo $item->address; ?>"
                                           placeholder="<?php echo $this->lang->line('경북 경산시 정평동 현대APT 107/1801'); ?>"/>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <a href="#" id="ng-submit" class="btn btn-primary pull-right" id="localize_530"><?php echo $this->lang->line('수정하기'); ?></a>
                        <?php
                        if ($item->isdeprecated) {
                            ?>
                            <a class="btn btn-danger pull-right" style="margin-right: 5px;"
                               href="<?= site_url('user/change_isdeprecated?userid=' . $item->_userid) . '&isdeprecated=false' ?>">
                                <i class="fa fa-credit-card"></i> <span id="localize_531"><?php echo $this->lang->line('살리기'); ?></span>
                            </a>
                            <?php
                        } else {
                            ?>
                            <a class="btn btn-success pull-right" style="margin-right: 5px;"
                               href="<?= site_url('user/change_isdeprecated?userid=' . $item->_userid) . '&isdeprecated=true' ?>">
                                <i class="fa fa-credit-card"></i> <span id="localize_532"><?php echo $this->lang->line('삭제하기'); ?></span>
                            </a>

                            <?php
                        }
                        ?>

                        <?php
                        if ($item->is_admin) {
                            ?>
                            <a class="btn btn-warning pull-right" style="margin-right: 5px;"
                               href="<?= site_url('user/change_admin?userid=' . $item->_userid) . '&isadmin=false' ?>">
                                <i class="fa fa-file-excel-o"></i> <span id="localize_533"><?php echo $this->lang->line('관리자 박탈'); ?></span>
                            </a>
                            <?php
                        } else {
                            ?>
                            <a class="btn btn-warning pull-right" style="margin-right: 5px;"
                               href="<?= site_url('user/change_admin?userid=' . $item->_userid) . '&isadmin=true' ?>">
                                <i class="fa fa-file-excel-o"></i> <span id="localize_533"><?php echo $this->lang->line('관리자 부여'); ?></span>
                            </a>

                            <?php
                        }
                        ?>
                        <a class="btn btn-primary pull-right" style="margin-right: 5px;"
                           href="<?= site_url('user/detail?userid=' . $item->_userid) ?>">
                            <i class="fa fa-download"></i><span id="localize_534"><?php echo $this->lang->line('뒤로가기'); ?></span>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>