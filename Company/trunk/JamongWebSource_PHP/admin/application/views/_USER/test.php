

<div class="content-wrapper">
    <section class="content-header">
        <h1 id="user_test_01">
            <?php echo $this->lang->line('메인 슬라이드'); ?>
        </h1>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-danger">
                    <div class="form-horizontal">
                        <form id="aw-form"
                              class="box-body" method="post" enctype="multipart/form-data"
                              action="<?= site_url('/user/test_upload')?>">
                            <input type="file" name="test">
                            <input type="submit" value="test">
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>