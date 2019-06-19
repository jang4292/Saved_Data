<?php
defined('BASEPATH') OR exit('No direct script access allowed');

//require('/var/www/html/admin/static/aws/aws-autoloader.php'); //이거 풀어야함

//use Aws\Ses\SesClient;

class Auth extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->load->model('owner_model');
        $this->load->library('form_validation');
    }

    function index()
    {
        redirect('/auth/login');
    }

    function login()
    {
        $this->__is_logined();
        $this->form_validation->set_rules('email', $this->lang->line('이메일'), 'required|valid_email');
        $this->form_validation->set_rules('password', $this->lang->line('비밀번호'), 'required');
        $isValidate = $this->form_validation->run();
        $email = $this->input->post('email');
        $password = $this->input->post('password');
        if ($isValidate)
        {
            $input_data = array('user_id' => $email);
            $rtv = $this->owner_model->get_user_by_email($input_data['user_id']);
            if ($rtv != null && count($rtv) > 0) {
                if ($password == $rtv[0]->user_password) {
                    $this->handle_login($rtv[0]);
                }
                else {
                    $this->session->set_flashdata('message', $this->lang->line('로그인에 실패하였습니다.'));
                    redirect('auth/login');
                }
            }
            else {
                $this->session->set_flashdata('message', $this->lang->line('잘못된 이메일 정보 입니다.'));
                $this->__get_views('_AUTH/login');
            }
        }
        else {
            $this->__get_views('_AUTH/login', array('returnURL' => $this->input->get('returnURL'), ));
        }
    }
    
    function logout()
    {
        $this->session->sess_destroy();
        redirect('/auth/login');
    }

    function handle_login($user)
    {
        $this->session->set_flashdata('message', $this->lang->line('로그인에 성공하였습니다.'));
        $this->session->set_userdata('user_uid', $user->user_uid);
        $this->session->set_userdata('user_id', $user->user_id);
        $this->session->set_userdata('user_name', $user->user_name);
        $this->session->set_userdata('user_auth', $user->user_auth);
        $this->session->set_userdata('user_mileage', $user->user_mileage);
        $this->session->set_userdata('user_tax_price', $user->user_tax_price);
        $this->session->set_userdata('is_login', true);
        $returnURL = $this->input->get('returnURL');
        if ($returnURL === false || $returnURL === "") {
            redirect('home/index');
        }
        redirect($returnURL);
    }

    /* submit 회원가입 */
    function submit_join()
    {
        $user_name = $this->input->post('user_name'); //유저이름 - 출력됨
        $user_id = $this->input->post('user_id'); //유저이메일 - 출력됨 jm
        $password = $this->input->post('password');
        $password_confirm = $this->input->post('password_confirm');

        //echo $user_name;
        //echo $user_id;
        $rtv = $this->owner_model->get_user_id_by_Name($user_name);//jm
        //nickName 중복?
        if (!count($rtv)) {
            $rtv = $this->owner_model->get_user_id_by_email($user_id);
            //email 중복?
            if (!count($rtv)) {
                if (!strcmp($password, $password_confirm)) {
                    require_once 'UUID.php';
                    $uuid = UUID::v4();
                    $input_data = array(
                        "user_name" => $user_name,
                        "user_id" => $user_id,
                        'uuid'  => $uuid,
                        "user_password" => $this->keyEncrypt($password));

                    $rtv_1 = $this->owner_model->add($input_data);
                    $rtv_2 = $this->owner_model->add_nickname($rtv_1, $input_data);

                    if ($rtv_1 && $rtv_2) {
                        $this->session->set_flashdata('message', $this->get_localize_text('회원등록에 성공 했습니다.'));
                        redirect('/user/user_register');

                    } else {
                        $this->session->set_flashdata('message', $this->lang->line('회원등록에 실패 했습니다.'));
                        redirect('/user/user_register');
                    }
                } else {
                    $this->session->set_flashdata('message', $this->lang->line('비밀번호가 일치하지 않습니다.'));
                    redirect('/user/user_register');
                }
            } else {
                $this->session->set_flashdata('message', $this->lang->line('이미 존재하는 이메일 입니다'));
                redirect('/user/user_register');
            }

        } else {
            $this->session->set_flashdata('message', $this->lang->line('이미 존재하는 이름 입니다'));
            redirect('/user/user_register');
        }
    }
}




