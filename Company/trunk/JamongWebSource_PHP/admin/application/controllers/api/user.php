<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

//require(realpath($_SERVER["DOCUMENT_ROOT"]) . '/admin/static/aws/aws-autoloader.php') ;

//require('/var/www/html/admin/static/aws/aws-autoloader.php');

//use Aws\S3\S3Client;
//use Aws\S3\Exception\S3Exception;

class User extends CORE_Controller {
    function __construct()
    {
        parent::__construct();
        $this->load->model('owner_model');
    }

    function test() {
        $rtv = array(
            'channels' => $this->input->post('channels'),
            'userId' => $this->input->post('userId')
        );
        echo json_encode($rtv, JSON_PRETTY_PRINT);
    }

    function get_users()
    {
        $page = $this->input->get('page');
        $per_page = $this->input->get('count');
        $sort = $this->input->get('sorting');
        $filter = $this->input->get('filter');

        if ($page === false || $per_page === false) {
            $page = 1;
            $per_page = 20;
        }

        $users = $this->owner_model->gets_pagination($page, $per_page, $sort, $filter);
        $total_count = $this->owner_model->get_total_count();

        $rtv = array(
            'row_count' => $total_count,
            'items' => $users,
        );
        echo json_encode($rtv, JSON_PRETTY_PRINT);
	//echo $per_page;
    }

    function detail()
    {
        $user_id = $this->input->get('userId');
        $user = $this->owner_model->get_user_by_uid($user_id);
        if ($user != null && count($user) > 0) {
            echo json_encode($user[0], JSON_PRETTY_PRINT);
        }
    }

    function update_user_auth () { // bit 수 업데이트

        $user_id = $this->input->get('userId');
        $user = $this->owner_model->get_userNumber($user_id);

        $json = json_encode($user[0], JSON_PRETTY_PRINT);
        $obj = json_decode($json);
        //$auth_bit = $obj->{'auth_bit'}; // 최초에 auth_bit = '0'
        $num = $obj->{'index'};

        $total = 0;
        foreach($_POST['permission'] as $dot) $total += $dot;
        //echo "$total";
        $this->owner_model->update_user_auth($total, $num);
        $this->session->set_flashdata('message', '타입 권한이 부여되었습니다. 해당 사용자가 재로그인한 경우 확인할 수 있습니다.이 기능은 권한이 부여된 경우만 확인할 수 있습니다.');
        redirect('user/detail?userId='.$num);
        //redirect('user/index');
    }

    function change_password()
    {
        $user_id = $this->input->post('userId');
        $password = $this->input->post('password');

        if (strlen($password) == 0) {
            $this->session->set_flashdata('message', $this->lang->line('비밀 번호 길이가 짧습니다.'));
            redirect('user/detail?userId=' . $user_id);
        } else {
            $rtv = $this->owner_model->change_password($user_id, $password);

            if ($rtv) {
                $this->session->set_flashdata('message', $this->lang->line('비밀 번호를 변경하는데 성공 했습니다.'));
                redirect('user/detail?userId=' . $user_id);
            } else {
                $this->session->set_flashdata('message', $this->lang->line('비밀 번호를 변경하는데 오류가 발생했습니다.'));
                redirect('user/detail?userId=' . $user_id);
            }
        }
    }
}


