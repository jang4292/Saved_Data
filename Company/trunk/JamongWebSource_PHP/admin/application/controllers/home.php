<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

class Home extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->load->model('owner_model');
        $this->require_logined();
    }

    function index()
    {
        $user = $this->owner_model->get_user_by_email($this->session->userdata('user_id'));
        if ($user != null && count($user) > 0) {
            $this->__get_views('_HOME/index', array('item' => $user[0]), array('data' => $user[0]));
        }
        /*
        else {
            $this->session->sess_destroy();
            redirect('/auth/login');
        }
        */
    }

    function change_password()
    {
        $user_id = $this->input->post('userId');
        $password = $this->input->post('password');
        if (strlen($password) == 0) {
            $this->session->set_flashdata('message', $this->lang->line('비밀 번호 길이가 짧습니다.'));
        } else {
            $rtv = $this->owner_model->change_password($user_id, $password);
            if ($rtv) {
                $this->session->set_flashdata('message', $this->lang->line('비밀 번호를 변경하는데 성공 했습니다.'));
            } else {
                $this->session->set_flashdata('message', $this->lang->line('비밀 번호를 변경하는데 오류가 발생했습니다.'));
            }
        }
        redirect('home/index');
    }
}
