##!/bin/bash
function get_image_tag(){
  if [ "$CI_COMMIT_REF_SLUG" == "master" ]; then
    export TAG="latest";
  else
    export TAG=$CI_COMMIT_REF_SLUG;
  fi
}