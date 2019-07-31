<template>
  <mdb-row>
    <mdb-col col="1"></mdb-col>
    <mdb-col class="text-center">
      <standard-banner></standard-banner><br />
      <standard-banner></standard-banner><br />

      <div class="embed-responsive embed-responsive-1by1 z-depth-1-half">
        <iframe v-bind:src="iframeUrl" class="embed-responsive-item"></iframe>
      </div>
    </mdb-col>
    <mdb-col col="1"></mdb-col>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </mdb-row>
</template>

<script>
import { mdbRow, mdbCol, Btn } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, Btn },
  data() {
    return {
      iframeUrl: `https://www.kiwiwall.com/wall/yFiN5xUZTOelOGhc64RV6UxnKaceDY3G/${
        this.$store.state.userData[0].Id
      }`
    };
  },
  methods: {
    test: function() {}
  },
  mounted() {}
};
</script>
