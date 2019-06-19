<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper" ng-controller="UserCtrl">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502"><?php echo $this->lang->line('회원'); ?></h1>
        <div align="right">
            <?php
            if ($this->session->userdata('user_auth') == 0) {
                ?>
                <a style="background-color: #222d32; color: white;" type="button" class="btn btn-md"
                   href="<?= site_url('user/user_register') ?>">사용자등록</a>
                <?php
            }
            ?>
        </div>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <div class="box-body table-responsive table-container">
                        <table ng-table="tableParams" class="table table-bordered table-hover">
                            <tr ng-repeat="item in $data">
                                <td data-title="'순서'" sortable="'index'">
                                    {{item.index}}
                                </td>
                                <td id="localize_503" data-title="'계정식별값'">
                                    {{item.uid}}
                                </td>
                                <td id="localize_505" data-title="'계정'" sortable="'user_id'">
                                        <p> <?php
                                            if ($this->session->userdata('user_auth') == 0) {
                                            ?><a href="<?= site_url('user/detail?userId={{item.index}}') ?>"><?php }?>
                                                {{item.user_id}}</a></p>
                                </td>
                                <td data-title="'이름'" sortable="'user_name'">
                                    {{item.user_name}}
                                </td>
                                <td data-title="'소속총판계정'" sortable="'reseller_id'">
                                    {{item.reseller_id}}
                                </td>
                                <td width="13%" data-title="'권한'" sortable="'user_auth'">
                                    <a ng-if="item.user_auth == 0" type="button" class="btn-xs btn-success">최고</a>
                                    <a ng-if="item.user_auth % (2 * 1) >= 1" type="button" class="btn-xs btn-warning">총판</a>
                                    <a ng-if="item.user_auth % (2 * 2) >= 2" type="button" class="btn-xs btn-danger">영상</a>
                                    <a ng-if="item.user_auth % (2 * 4) >= 4" type="button" class="btn-xs btn-info">기기</a>
                                    <a ng-if="item.user_auth % (2 * 8) >= 8" type="button" class="btn-xs btn-default">점주</a>
                                </td>
                                <!--
                                <td width="7%" data-title="'권한변경'" style="text-align: center">
                                    <button style="background-color: #222d32; color: white;" type="button" class="btn btn-xs" data-toggle="modal" data-target="#{{item.index}}">변경하기</button>

                                    <div class="modal fade" id="{{item.index}}" role="dialog">
                                        <div class="modal-dialog">

                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                    <h4 class="modal-title">{{item.user_name}}의 권한 변경하기</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <ng-form action="<?= site_url('/api/user/update_user_auth?userId={{item.index}}') ?>" style="margin-top: 10px" method="post" selected>
                                                        <input type="checkbox" name="permission[]" value="1" >총판
                                                        <input type="checkbox" name="permission[]" value="2">영상
                                                        <input type="checkbox" name="permission[]" value="4">기기
                                                        <input type="checkbox" name="permission[]" value="8">점주
                                                        <input type="checkbox" name="permission[]" onclick=chkBox(this.checked)>복합
                                                        <input type="submit" class="btn btn-success" value="권한부여"/>
                                                    </ng-form>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                -->
                                <td id="localize_509" data-title="'적립금'" sortable="'user_mileage'">
                                    {{item.user_mileage}}
                                </td>
                                <td data-title="'가입일시'" sortable="'create_at'">
                                    {{item.create_at}}
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>