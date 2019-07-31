<template>
  <div>
    <mdb-row>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col class="text-center">
        <mdb-row>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        </mdb-row>
        <br />
        <br />

        <div class="text-center mt-4">
          <mdb-btn v-on:click="createSupportTicketsSection()" color="secondary"
            >Create a new ticket</mdb-btn
          >
        </div>
        <br />

        <div class="text-center mt-4">
          <mdb-btn v-on:click="viewSupportTicketsSection()" color="secondary"
            >View Support Tickets</mdb-btn
          >
        </div>
        <br />
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>

    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
    </mdb-row>

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
  </div>
</template>

<script>
import { mdbRow, mdbCol, mdbBtn } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, mdbBtn },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      message: null,
      subject: null,
      email: null,
      apiResponse: null,
      errorList: null,
      successList: null
    };
  },
  mounted() {},
  methods: {
    test: function() {},
    createSupportTicketsSection: function() {
      this.$router.push("HomeSupportTickets");
    },
    viewSupportTicketsSection: function() {
      this.$router.push("HomeViewTickets");
    }
  }
};
</script>
