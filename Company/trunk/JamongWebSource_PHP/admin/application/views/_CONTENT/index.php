<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper" ng-controller="ContentCtrl">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502">영상</h1>
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
                                <td data-title="'영상이름'">
                                    {{item.uid}}
                                </td>
                                <td data-title="'공급자계정'">
                                    {{item.cp_id}}
                                </td>
                                <td id="localize_507" data-title="'제목'">
                                    {{item.video_title}}
                                </td>
                                <td id="localize_507" data-title="'재생시간'">
                                    {{item.playtime}}
                                </td>
                                <td id="localize_507" data-title="'가격'" sortable="'version'">
                                    {{item.price}}
                                </td>
                                <td id="localize_507" data-title="'모션호환'" sortable="'motion_accept'">
                                    <a ng-if="item.motion_accept % (2 * 1) >= 1" type="button" class="btn-xs btn-warning">1종</a>
                                    <a ng-if="item.motion_accept % (2 * 2) >= 2" type="button" class="btn-xs btn-danger">2종</a>
                                    <a ng-if="item.motion_accept % (2 * 4) >= 4" type="button" class="btn-xs btn-info">3종</a>
                                    <a ng-if="item.motion_accept % (2 * 8) >= 8" type="button" class="btn-xs btn-default">4종</a>
                                </td>
                                <td data-title="'생성일시'" sortable="'create_at'">
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
