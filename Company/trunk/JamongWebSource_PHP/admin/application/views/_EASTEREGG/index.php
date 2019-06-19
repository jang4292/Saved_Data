<?php header('Content-Type: text/html; charset=UTF-8'); ?>
<div class="content-wrapper">
    <section class="content-header">
        <h1 id="localize_502">이스터에그 코호급 결투장</h1>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <div class="box-body">
                        <div class="col-md-4">
                            <input id="player_01" class="form-control" type="text">
                        </div>
                        <div class="col-md-4">
                            <input id="player_02" class="form-control" type="text">
                        </div>
                        <div class="col-md-4">
                            <a id="player_button" class="form-control btn btn-danger" type="button">능력치 추출</a>
                        </div>
                    </div>
                </div>
                <div class="box">
                    <div class="box-body">
                        <div class="col-sm-4">
                            <table>
                                <thead>
                                <th class="text-center" width="12.5%">공격력</th>
                                <th class="text-center" width="12.5%">방어력</th>
                                <th class="text-center" width="12.5%">생명력</th>
                                <th class="text-center" width="12.5%">민첩</th>
                                <th class="text-center" width="12.5%">운</th>
                                </thead>
                                <tbody>
                                <td class="text-center" id="player_01_att">0</td>
                                <td class="text-center" id="player_01_def">0</td>
                                <td class="text-center" id="player_01_hp">0</td>
                                <td class="text-center" id="player_01_dex">0</td>
                                <td class="text-center" id="player_01_luck">0</td>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-sm-4">
                            <table>
                                <thead>
                                <th class="text-center" width="12.5%">공격력</th>
                                <th class="text-center" width="12.5%">방어력</th>
                                <th class="text-center" width="12.5%">생명력</th>
                                <th class="text-center" width="12.5%">민첩</th>
                                <th class="text-center" width="12.5%">운</th>
                                </thead>
                                <tbody>
                                <td class="text-center" id="player_02_att">0</td>
                                <td class="text-center" id="player_02_def">0</td>
                                <td class="text-center" id="player_02_hp">0</td>
                                <td class="text-center" id="player_02_dex">0</td>
                                <td class="text-center" id="player_02_luck">0</td>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-md-4">
                            <a id="battle_button" class="form-control btn btn-danger" type="button">코호급 결투</a>
                        </div>
                    </div>
                </div>
                <div class="box">
                    <div class="box-body">
                        <div class="col-md-12 text-center">
			    <span id="battle_stage">
			    </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>
<script src="https://code.jquery.com/jquery-1.10.2.js"></script>
<script src="http://www.myersdaily.org/joseph/javascript/md5.js"></script>
<script>
    var turn_count = 1;
    var battle_interval;

    $('#battle_button').click(function (){
        battle_interval = setInterval(function(){
	    do_battle($('#player_01_dex').text(), $('#player_02_dex').text());
        }, 1000);
    });

    function do_battle(player01_dex, player02_dex){
	if(player01_dex >= player02_dex){
                $('#battle_stage').prepend('제 ' + turn_count + ' 합째<br>');
                do_attack('player_01', 'player_02');
                do_attack('player_02', 'player_01');
        }
        else{
                $('#battle_stage').prepend('제 ' + turn_count + ' 합째<br>');
                do_attack('player_02', 'player_01');
                do_attack('player_01', 'player_02');
        }
        $('#battle_stage').prepend('<br>');
	turn_count++;
	if(parseInt($('#player_01_hp').text()) <= 0 || parseInt($('#player_02_hp').text()) <= 0){
		clearInterval(battle_interval);
		$('#battle_stage').prepend('코호급 결투 종료!<br>승자: ' + (parseInt($('#player_01_hp').text() > 0) ? $('#player_01').val() : $('#player_02').val()) + ' 님');
		turn_count = 1;
	}
    }

    function do_attack(player01, player02){
        var att = Math.floor(Math.random() * parseInt($('#' + player01 + '_att').text())) + parseInt($('#' + player01 + '_att').text()) * parseInt($('#' + player01 + '_luck').text()) / 500;
        var def = Math.floor(Math.random() * parseInt($('#' + player02 + '_def').text())) + parseInt($('#' + player02 + '_def').text()) * parseInt($('#' + player02 + '_luck').text()) / 50;
        var damage = ((att - def) > 0 ? (att - def) : 0);
        $('#' + player02 + '_hp').text($('#' + player02 + '_hp').text() - (parseInt(damage)));
	$('#battle_stage').prepend(
	    $('#' + player01).val() + '이(가) ' + $('#' + player02).val() + '게(에게) ' + parseInt(damage) + '만큼의 대미지를 입혔당!<br>'
	);
    }

    $('#player_button').click(function (){
        var count = 0;
        var acc = 0;
        var pos = 0;
        $.each(md5($('#player_01').val()).split(''), function (i, v){
            if(count < 4){
                acc += parseInt(v, 16);
                count++;
            }
            else{
                if(pos == 0){
                    $('#player_01_att').text(acc);
                }
                if(pos == 1){
                    $('#player_01_def').text(acc);
                }
                if(pos == 2){
                    $('#player_01_hp').text(acc);
                }
                if(pos == 3){
                    $('#player_01_dex').text(acc);
                }
                if(pos == 4){
                    $('#player_01_luck').text(acc);
                }
                count = 0;
                acc = 0;
                pos++;
            }
        });

        pos = 0;
        $.each(md5($('#player_02').val()).split(''), function (i, v){
            if(count < 4){
                acc += parseInt(v, 16);
                count++;
            }
            else{
                if(pos == 0){
                    $('#player_02_att').text(acc);
                }
                if(pos == 1){
                    $('#player_02_def').text(acc);
                }
                if(pos == 2){
                    $('#player_02_hp').text(acc);
                }
                if(pos == 3){
                    $('#player_02_dex').text(acc);
                }
                if(pos == 4){
                    $('#player_02_luck').text(acc);
                }
                count = 0;
                acc = 0;
                pos++;
            }
        });
    });
</script>
