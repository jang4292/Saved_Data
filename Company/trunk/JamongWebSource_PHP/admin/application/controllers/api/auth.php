<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

//require('/var/www/html/admin/static/aws/aws-autoloader.php');

//use Aws\S3\S3Client;
//use Aws\S3\Exception\S3Exception;

class Auth extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->load->model('owner_model');
    }

    function login()
    {
        $email = $this->input->post('user_id');
        $password = $this->input->post('user_password');

        $user = $this->owner_model->get_user_by_email(array('user_id' => $email));

        if ($user != null && $user->email == $email &&
            $this->keyEncrypt($password) == $user->password) {
            echo json_encode($user->userNumber, JSON_PRETTY_PRINT);
        } else {
            echo json_encode(-1, JSON_PRETTY_PRINT);
        }
    }
}
