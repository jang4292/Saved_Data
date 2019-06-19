<?php
/**
 * Created by PhpStorm.
 * User: Administrator
 * Date: 2016-10-18
 * Time: 오후 4:28
 */

class LanguageLoader
{
    function initialize() {
        $ci =& get_instance();
        $ci->load->helper('language');
        $siteLang = $ci->session->userdata('site_lang');
        if ($siteLang) {
            $ci->lang->load('message',$siteLang);
        } else {
            $ci->lang->load('message','korean');
        }
    }
}