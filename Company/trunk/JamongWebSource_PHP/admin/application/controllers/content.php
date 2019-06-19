<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

class Content extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->__require_admin_login();
        $this->load->model('content_model');
    }

    function index()
    {
        $this->__get_views('_CONTENT/index.php', array());
    }

    function detail()
    {
        $userid = $this->input->get('userId');
        $user = $this->content_model->get_user_by_id($userid);
        if ($user != null && count($user) > 0) {
            $this->__get_views('_USER/detail.php', array('item' => $user[0]));
        } else {
            $this->session->set_flashdata('message', $this->lang->line('해당 유저가 없습니다.'));
            redirect('user/index');
        }
    }
}
