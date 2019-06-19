<?php

class Content_model extends CI_Model
{
    private $table;

    function __construct() {
        parent::__construct();
        $this->table = 'content';
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
        $this->db->select('a.*, b.user_id as cp_id');
        $this->db->from('content as a');
        $this->db->join('owner as b', 'a.owner_uid = b.uid', 'left');

        // sorting
        if (isset($sort['index'])) $this->db->order_by("index", $sort['index']);
        if (isset($sort['uid'])) $this->db->order_by("uid", $sort['uid']);
        if (isset($sort['cp_id'])) $this->db->order_by("cp_id", $sort['cp_id']);
        if (isset($sort['video_title'])) $this->db->order_by("video_title", $sort['video_title']);
        if (isset($sort['video_playtime'])) $this->db->order_by("video_playtime", $sort['video_playtime']);
        if (isset($sort['motion_accept'])) $this->db->order_by("motion_accept", $sort['motion_accept']);
        if (isset($sort['price'])) $this->db->order_by("price", $sort['price']);
        if (isset($sort['tax_price'])) $this->db->order_by("tax_price", $sort['tax_price']);
        if (isset($sort['create_at'])) $this->db->order_by("create_at", $sort['create_at']);
        if (isset($sort['update_at'])) $this->db->order_by("update_at", $sort['update_at']);
        if (isset($sort['delete_at'])) $this->db->order_by("delete_at", $sort['delete_at']);

        // filter
        if ($filter != null && isset($filter['index'])) $this->db->like('index', $filter['index']);
        if ($filter != null && isset($filter['uid'])) $this->db->like('uid', $filter['uid']);
        if ($filter != null && isset($filter['cp_id'])) $this->db->like('cp_id', $filter['cp_id']);
        if ($filter != null && isset($filter['video_title'])) $this->db->like('video_title', $filter['video_title']);
        if ($filter != null && isset($filter['video_playtime'])) $this->db->like('video_playtime', $filter['video_playtime']);
        if ($filter != null && isset($filter['motion_accept'])) $this->db->like('motion_accept', urldecode($filter['motion_accept']));
        if ($filter != null && isset($filter['price'])) $this->db->like('price', urldecode($filter['price']));
        if ($filter != null && isset($filter['tax_price'])) $this->db->like('tax_price', urldecode($filter['tax_price']));
        if ($filter != null && isset($filter['create_at'])) $this->db->like('create_at', urldecode($filter['create_at']));

        return $this->db->get()->result();
    }

    function get_total_count()
    {
        return $this->db->count_all($this->table);
    }

    function get_content_by_uid($content_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('uid', $content_uid);

        return $this->db->get()->result();
    }

    function get_machine_by_owner_uid($owner_uid)
    {
        $this->db->select('*');
        $this->db->from($this->table);
        $this->db->where('owner_uid', $owner_uid);

        return $this->db->get()->result();
    }
}
