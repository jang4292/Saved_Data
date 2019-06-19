<?php

class Machine_model extends CI_Model
{
    private $table;

    function __construct() {
        parent::__construct();
        $this->table = 'machine';
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
        $this->db->select('a.*, b.user_id as owner_id, c.user_id as ap_id');
        $this->db->from('machine as a');
        $this->db->join('owner as b', 'a.owner_uid = b.uid', 'left');
        $this->db->join('owner as c', 'a.ap_uid = c.uid', 'left');

        // sorting
        if (isset($sort['index'])) $this->db->order_by("index", $sort['index']);
        if (isset($sort['uid'])) $this->db->order_by("uid", $sort['uid']);
        if (isset($sort['owner_id'])) $this->db->order_by("owner_id", $sort['owner_id']);
        if (isset($sort['ap_id'])) $this->db->order_by("ap_id", $sort['ap_id']);
        if (isset($sort['place_at'])) $this->db->order_by("place_at", $sort['place_at']);
        if (isset($sort['version'])) $this->db->order_by("version", $sort['version']);
        if (isset($sort['motion_accept'])) $this->db->order_by("motion_accept", $sort['motion_accept']);
        if (isset($sort['tax_price'])) $this->db->order_by("tax_price", $sort['tax_price']);
        if (isset($sort['create_at'])) $this->db->order_by("create_at", $sort['create_at']);
        if (isset($sort['update_at'])) $this->db->order_by("update_at", $sort['update_at']);
        if (isset($sort['delete_at'])) $this->db->order_by("delete_at", $sort['delete_at']);

        // filter
        if ($filter != null && isset($filter['index'])) $this->db->like('index', $filter['index']);
        if ($filter != null && isset($filter['uid'])) $this->db->like('uid', $filter['uid']);
        if ($filter != null && isset($filter['owner_id'])) $this->db->like('owner_id', $filter['owner_id']);
        if ($filter != null && isset($filter['ap_id'])) $this->db->like('ap_id', $filter['ap_id']);
        if ($filter != null && isset($filter['place_at'])) $this->db->like('place_at', $filter['place_at']);
        if ($filter != null && isset($filter['version'])) $this->db->like('version', urldecode($filter['version']));
        if ($filter != null && isset($filter['motion_accept'])) $this->db->like('motion_accept', urldecode($filter['motion_accept']));
        if ($filter != null && isset($filter['tax_price'])) $this->db->like('tax_price', urldecode($filter['tax_price']));
        if ($filter != null && isset($filter['create_at'])) $this->db->like('create_at', urldecode($filter['create_at']));

        return $this->db->get()->result();
    }

    function get_total_count()
    {
        return $this->db->count_all($this->table);
    }

    function get_machine_by_uid($machine_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('uid', $machine_uid);

        return $this->db->get()->result();
    }

    function get_machine_by_owner_uid($owner_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('owner_uid', $owner_uid);

        return $this->db->get()->result();
    }

    function get_machine_by_ap_uid($ap_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('ap_uid', $ap_uid);

        return $this->db->get()->result();
    }
}
