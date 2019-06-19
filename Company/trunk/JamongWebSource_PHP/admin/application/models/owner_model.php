<?php

class Owner_model extends CI_Model
{
    private $table;

    function __construct() {
        parent::__construct();
        $this->table = 'owner';
    }

    function gets() {
        $this->db->select('*');
        $this->db->from($this->table);
        return $this->db->get()->result();
    }

    function gets_pagination($page, $per_page, $sort, $filter) {
        if ($page === 1) {
            $this->db->limit($per_page);
        } else {
            $this->db->limit($per_page, ($page - 1) * $per_page);
        }
        $this->db->select('a.*, b.user_id reseller_id');
        $this->db->from('owner a');
        $this->db->join('owner b', 'a.reseller_uid = b.uid', 'left');
        $this->db->order_by('a.index', 'asc');

        // sorting
        if (isset($sort['index'])) $this->db->order_by("index", $sort['index']);
        if (isset($sort['uid'])) $this->db->order_by("uid", $sort['uid']);
        if (isset($sort['user_id'])) $this->db->order_by("user_id", $sort['user_id']);
        if (isset($sort['reseller_id'])) $this->db->order_by("reseller_id", $sort['reseller_id']);
        if (isset($sort['user_name'])) $this->db->order_by("user_name", $sort['user_name']);
        if (isset($sort['user_auth'])) $this->db->order_by("user_auth", $sort['user_auth']);
        if (isset($sort['user_mileage'])) $this->db->order_by("user_mileage", $sort['user_mileage']);
        if (isset($sort['tax_price'])) $this->db->order_by("tax_price", $sort['tax_price']);
        if (isset($sort['create_at'])) $this->db->order_by("create_at", $sort['create_at']);
        if (isset($sort['update_at'])) $this->db->order_by("update_at", $sort['update_at']);
        if (isset($sort['delete_at'])) $this->db->order_by("delete_at", $sort['delete_at']);

        // filter
        if ($filter != null && isset($filter['index'])) $this->db->like('index', $filter['index']);
        if ($filter != null && isset($filter['user_id'])) $this->db->like('user_id', $filter['user_id']);
        if ($filter != null && isset($filter['reseller_id'])) $this->db->like('reseller_id', $filter['reseller_id']);
        if ($filter != null && isset($filter['user_name'])) $this->db->like('user_name', urldecode($filter['user_name']));
        if ($filter != null && isset($filter['user_auth'])) $this->db->like('user_auth', urldecode($filter['user_auth']));
        if ($filter != null && isset($filter['tax_price'])) $this->db->like('tax_price', urldecode($filter['tax_price']));
        if ($filter != null && isset($filter['create_at'])) $this->db->like('create_at', urldecode($filter['create_at']));

        return $this->db->get()->result();
    }

    function get_total_count()
    {
        return $this->db->count_all($this->table);
    }

    function get_user_by_email($user_id)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('user_id', $user_id);

        return $this->db->get()->result();
    }

    function get_user_by_uid($user_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('index', $user_uid);

        return $this->db->get()->result();
    }

    function check_user_admin($user_uid) {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('uid', $user_uid);

        return $this->db->get()->result();
    }

    function change_password($user_id, $user_password)
    {
        try {
            $data = array(
                'user_password' => $user_password
            );
            $this->db->where('owner.index', $user_id);
            $this->db->update($this->table, $data);
            return $this->db->affected_rows();
        } catch (Exception $e) {
            return false;
        }
    }
    function update_user_auth($auth_bit, $num){

        //$query_str = "update jamong__tb_users set auth_bit = 1 | 0 where userNumber = '.$user_id.'";
        //$query_str = "update jamong__tb_users set auth_bit = 1 | 0 where userNumber = 233";
        $query_str = "update owner set user_auth = {$auth_bit} | 0 where owner.index = {$num}";
        //return $query_str;

        $query = $this->db->query($query_str);
        return $query;

    } //권한 부여

    function get_userNumber($user_id){
        $query_str ="SELECT * FROM  owner".
            " WHERE owner.index='$user_id'";

        $query = $this->db->query($query_str);
        return $query->result();
    } //유저정보불러옴
}


function get_user_id_by_Name($user_name){ //사용자 이름
    try{
        $this->db->select('*');
        $this->db->where('user_name' , $user_name);
        $this->db->from('owner');
        return $this->db->get()->result();
    }
    catch(Exception $e){
        return $e;
    }

}



function get_user_id_by_email($user_id){ //이메일
    try {

        $this->db->select('*');
        $this->db->where('user_id' ,  $user_id);
        $this->db->from('owner');
        $rtv = $this->db->get()->result();
        return $rtv;
    } catch (Exception $e) {
        return $e;
    }
}


function add($data)
{
    $input_data = array(
        'user_id' => $data['user_id'],
        'user_password' => $data['password'],
        'uuid' => $data['uuid'],
        'create_at' => date("y-m-d H:i:s")
    );

    $this->db->insert($this->table, $input_data);
    $result = $this->db->insert_id();

    return $result;
}

function add_nickName($userId, $data){
    $input_data = array(
        'user_name' => $data['user_name'],
        'index' => $userId
    );

    $this->db->insert($this->table, $input_data);
    $result = $this->db->insert_id();

    return $result;
}