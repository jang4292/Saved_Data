<?php

class Machine_content_model extends CI_Model
{
    private $table;

    function __construct() {
        parent::__construct();
        $this->table = 'machine_content';
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
        $this->db->select('a.*, b.video_title as video_title');
        $this->db->from('machine_content as a');
        $this->db->join('content as b', 'a.content_uid = b.uid', 'left');

        // sorting
        if (isset($sort['index'])) $this->db->order_by("index", $sort['index']);
        if (isset($sort['machine_uid'])) $this->db->order_by("machine_uid", $sort['machine_uid']);
        if (isset($sort['video_title'])) $this->db->order_by("content_uid", $sort['video_title']);
        if (isset($sort['price'])) $this->db->order_by("price", $sort['price']);
        if (isset($sort['create_at'])) $this->db->order_by("create_at", $sort['create_at']);
        if (isset($sort['update_at'])) $this->db->order_by("update_at", $sort['update_at']);
        if (isset($sort['delete_at'])) $this->db->order_by("delete_at", $sort['delete_at']);

        // filter
        if ($filter != null && isset($filter['index'])) $this->db->like('index', $filter['index']);
        if ($filter != null && isset($filter['machine_uid'])) $this->db->like('machine_uid', $filter['machine_uid']);
        if ($filter != null && isset($filter['video_title'])) $this->db->like('video_title', $filter['video_title']);
        if ($filter != null && isset($filter['price'])) $this->db->like('price', $filter['price']);
        if ($filter != null && isset($filter['create_at'])) $this->db->like('create_at', urldecode($filter['create_at']));

        return $this->db->get()->result();
    }

    function get_total_count()
    {
        return $this->db->count_all($this->table);
    }

    function get_machine_content_by_machine_uid($machine_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('machine_uid', $machine_uid);

        return $this->db->get()->result();
    }

    function get_machine_content_by_owner_content_uid($content_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('content_uid', $content_uid);

        return $this->db->get()->result();
    }
}
