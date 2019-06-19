<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

class User extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->__require_admin_login();
        $this->load->model('owner_model');
    }

    function index()
    {
        $this->__get_views('_USER/index.php', array());
    }
    function user_register()
    {
        $this->__get_views('_USER/user_register.php');
    }

    function detail()
    {
        $userid = $this->input->get('userId');
        $user = $this->owner_model->get_user_by_uid($userid);
        if ($user != null && count($user) > 0) {
            $this->__get_views('_USER/detail.php', array('item' => $user[0]));

        } else {
            $this->session->set_flashdata('message', $this->lang->line('해당 유저가 없습니다.'));
            redirect('user/index');
        }
    }


    function update()
    {
        $userid = $this->input->get('userid');
        $user = $this->owner_model->get_user_by_id($userid);
        if ($user != null) {
            $this->__get_views('_USER/update.php', array('item' => $user));

        } else {
            $this->session->set_flashdata('message', $this->lang->line('해당 유저가 없습니다.'));
            redirect('user/index');
        }
    }

    function change_admin()
    {
        $userid = $this->input->get('userid');
        $isadmin = $this->input->get('isadmin') == 'true' ? true : false;

        if (!$userid) {
            $this->session->set_flashdata('message', $this->lang->line('페이지를 로드하는데 오류가 발생했습니다.'));
        } else {
            $rtv = $this->owner_model->change_admin($userid, $isadmin);

            if ($rtv == 1) {
                $this->session->set_flashdata('message', $this->lang->line('관리가 권한 변경이 성공했습니다.'));
            } else {
                $this->session->set_flashdata('message', $this->lang->line('관리가 권한 변경에 오류가 발생했습니다.'));
            }
        }
        redirect('/user/detail?userid='.$userid);
    }

    function change_isdeprecated()
    {
        $userid = $this->input->get('userid');
        $isdeprecated = $this->input->get('isdeprecated') == 'true' ? true : false;

        $rtv = $this->owner_model->change_isdeprecated($userid, $isdeprecated);
        if ($rtv) {
            if ($isdeprecated) {
                $this->session->set_flashdata('message', $this->lang->line('사용자를 성공적으로 삭제하였습니다.'));
            } else {
                $this->session->set_flashdata('message', $this->lang->line('사용자를 성공적으로 부활하였습니다.'));
            }

            redirect('user/index');
        } else {
            if ($isdeprecated) {
                $this->session->set_flashdata('message', $this->lang->line('사용자를 삭제하는데 오류가 발생했습니다. 개발자에게 문의하세요.'));
            } else {
                $this->session->set_flashdata('message', $this->lang->line('사용자를 부활하는데 오류가 발생했습니다. 개발자에게 문의하세요.'));
            }

            redirect('user/detail?userid='.$userid);
        }
    }

    




}
