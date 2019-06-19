<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper" ng-controller="LogsCtrl">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502">이용 기록</h1>
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
                                    {{item.machine_uid}}
                                </td>
                                <td id="localize_507" data-title="'영상이름'">
                                    {{item.video_title}}
                                </td>
                                <td id="localize_507" data-title="'이용객수'">
                                    {{item.costomers}}
                                </td>
                                <td id="localize_507" data-title="'상태'">
                                    <span ng-if="item.status == 0">정상</span>
                                    <span ng-if="item.status == 1">오류</span>
                                    <span ng-if="item.status == 2">비상정지</span>
                                    <span ng-if="item.status == 3">고장</span>
                                </td>
                                <td id="localize_507" data-title="'가격'">
                                    {{item.price}}
                                </td>
                                <td id="localize_507" data-title="'시작일시'" sortable="'play_at'">
                                    {{item.play_at}}
                                </td>
                                <td id="localize_507" data-title="'종료일시'" sortable="'end_at'">
                                    {{item.end_at}}
                                </td>
                                <td data-title="'이용일시'" sortable="'create_at'">
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
