<?php if (!defined('BASEPATH')) exit('No direct script access allowed');

class Easteregg extends CORE_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->require_logined();
    }

    function index()
    {
        $this->__get_views('_EASTEREGG/index', array(), array());
    }

}
