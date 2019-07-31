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

        <mdb-row>
          <mdb-col col="3"></mdb-col>
          <mdb-col class="text-center">
            <mdb-card border="secondary" class="mb-3" style="max-width: 18rem;">
              <mdb-card-header class="text-secondary"
                >View Support Ticket</mdb-card-header
              >
              <mdb-card-body class="text-secondary">
                <mdb-card-text>
                  <p class="font-weight-bold">Reference#</p>
                  <input
                    v-validate="'required'"
                    name="reference"
                    v-model.trim="reference"
                    type="text"
                    class="form-control"
                  />

                  <span class="font-weight-bold">
                    {{ errors.first("reference") }} <br />
                    <span v-if="doesntExist"
                      >You do not have any such referenced support ticket.
                    </span>
                  </span>

                  <div class="text-center mt-4">
                    <mdb-btn v-on:click="viewSupportTicket()" color="secondary"
                      >View</mdb-btn
                    >
                  </div>
                </mdb-card-text>
              </mdb-card-body>
            </mdb-card>
          </mdb-col>
          <mdb-col col="3"></mdb-col>
        </mdb-row>

        <br />

        <mdb-datatable v-bind:data="data" striped bordered class="w-75" />
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
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
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbDatatable,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText
} from "mdbvue";

export default {
  components: {
    mdbRow,
    mdbCol,
    mdbBtn,
    mdbDatatable,
    mdbCard,
    mdbCardHeader,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      errorList: null,
      successList: null,
      reference: null,
      subject: null,
      message: null,
      doesntExist: false,
      data: {
        columns: [
          {
            label: "Reference",
            field: "reference",
            sort: "asc"
          },
          {
            label: "Created",
            field: "created",
            sort: "asc"
          },
          {
            label: "Subject",
            field: "subject",
            sort: "asc"
          },
          {
            label: "Status",
            field: "status",
            sort: "asc"
          }
        ],
        rows: []
      }
    };
  },
  sockets: {
    receivingSupportTickets: function(data) {
      this.data.rows.length = 0;
      for (var i = 0; i < Object.keys(data).length; i++) {
        var status;
        if (data[i].Locked === "true") {
          status = "Closed";
        } else if (data[i].LastReplier === "admin") {
          status = "Answered";
        } else {
          status = "Open";
        }
        this.data.rows.push({
          reference: data[i].Reference,
          created: this.moment(data[i].DateTime).format(
            "YYYY-MM-DD - hh:mm:ss A"
          ),
          subject: data[i].Subject,
          status: status
        });
      }

      this.$store.commit("isLoading", false);
    },
    createdSupportTicket: function(data) {
      this.$socket.emit(
        "requestingSupportTickets",
        JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
      );
    }
  },
  methods: {
    test: function() {
      //console.log(this.data.rows[0].reference);

      for (var i = 0; i < this.data.rows.length; i++) {
        console.log(this.reference === this.data.rows[i].reference);
      }
    },
    viewSupportTicket: function() {
      this.$store.commit("isLoading", true);
      this.doesntExist = false;
      var hasAny = false;
      this.$validator.validateAll().then(result => {
        if (result === true) {
          for (var i = 0; i < this.data.rows.length; i++) {
            if (this.reference === this.data.rows[i].reference) {
              hasAny = true;
              break;
            }
          }

          if (hasAny === true) {
            this.$store.commit("viewTicketsReferenceChange", this.reference);
            this.$router.push("/HomeSupportTicketsReply");
          } else if (hasAny === false) {
            this.doesntExist = true;
          }
        }
      });
      this.$store.commit("isLoading", false);
    }
  },
  mounted() {
    this.$store.commit("isLoading", true);
    this.$socket.emit(
      "requestingSupportTickets",
      JSON.stringify({ sessionId: this.$cookie.get("sessionId") })
    );

    this.$store.commit("isLoading", false);
  }
};
</script>
