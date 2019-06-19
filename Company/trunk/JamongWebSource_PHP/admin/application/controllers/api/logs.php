<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

//require(realpath($_SERVER["DOCUMENT_ROOT"]) . '/admin/static/aws/aws-autoloader.php') ;

//require('/var/www/html/admin/static/aws/aws-autoloader.php');

//use Aws\S3\S3Client;
//use Aws\S3\Exception\S3Exception;

class Logs extends CORE_Controller {
    function __construct()
    {
        parent::__construct();
        $this->load->model('logs_model');
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

        $users = $this->logs_model->gets_pagination($page, $per_page, $sort, $filter);
        $total_count = $this->logs_model->get_total_count();

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
        $user = $this->logs_model->get_user_by_id($user_id);
        if ($user != null && count($user) > 0) {
            echo json_encode($user[0], JSON_PRETTY_PRINT);
        }
    }
}