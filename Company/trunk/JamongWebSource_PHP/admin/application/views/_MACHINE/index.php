<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper" ng-controller="MachineCtrl">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502">기기</h1>
        <div align="right">
            <?php
            if ($this->session->userdata('user_auth') == 0) {
                ?>
                <a style="background-color: #222d32; color: white;" type="button" class="btn btn-md"
                   href="">기기등록</a>
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
                                <td id="localize_505" data-title="'기기식별값'">
                                    {{item.uid}}
                                </td>
                                <td id="localize_507" data-title="'점주계정'">
                                    {{item.owner_id}}
                                </td>
                                <td id="localize_507" data-title="'공급자계정'">
                                    {{item.ap_id}}
                                </td>
                                <td id="localize_507" data-title="'위치'">
                                    {{item.place_at}}
                                </td>
                                <td id="localize_507" data-title="'버전'" sortable="'version'">
                                    {{item.version}}
                                </td>
                                <td id="localize_507" data-title="'모션호환'" sortable="'motion_accept'">
                                    <a ng-if="item.motion_accept % (2 * 1) >= 1" type="button" class="btn-xs btn-warning">1종</a>
                                    <a ng-if="item.motion_accept % (2 * 2) >= 2" type="button" class="btn-xs btn-danger">2종</a>
                                    <a ng-if="item.motion_accept % (2 * 4) >= 4" type="button" class="btn-xs btn-info">원동기</a>
                                    <a ng-if="item.motion_accept % (2 * 8) >= 8" type="button" class="btn-xs btn-default">대형</a>
                                    <a ng-if="item.motion_accept % (2 * 16) >= 16" type="button" class="btn-xs btn-default">특수</a>
                                </td>
                                <td data-title="'설치일시'" sortable="'create_at'">
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
