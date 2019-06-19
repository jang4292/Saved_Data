<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper" ng-controller="MachineContentCtrl">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1 id="localize_502">기기별 영상 구매 내역</h1>
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
                                <td id="localize_507" data-title="'가격'">
                                    {{item.price}}
                                </td>
                                <td data-title="'구매일시'" sortable="'create_at'">
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