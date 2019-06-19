<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

class Balance extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->__require_admin_login();
        $this->load->model('logs_model');
    }

    function index()
    {
        $this->__get_views('_BALANCE/index.php', array('array_balance' => $this->logs_model->get_balance_within_date()));
    }
}
